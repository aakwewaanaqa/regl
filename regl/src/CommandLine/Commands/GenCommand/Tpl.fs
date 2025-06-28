module Regl.CommandLine.Commands.GenCommand.Tpl

open Regl.CommandLine.Commands.GenCommand.Types.Lines
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.GenCommand.Shared
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds
open Regl.CommandLine.Types.FlagsAndParams

let cmdName = "tpl"

let cmdInfo = "reads context and execute bash template file then redirect its stdout to parent stdout for output"

let entry =
    /// param of reading line count
    let lineCountParam =
        IntParam ("line-count", "reading lines with a specific count of line to stdout")
    /// param of the template bash file
    let bashFileParam =
        Param("template-file", "the template files to echos stdout for generated file")
    /// flag to start reading
    let startFlag =
        BoolFlag("--start", "starts reading for source file context")
    /// flag to stop reading
    let endFlag =
        BoolFlag("--end", "ends reading for source file context")
    /// `tpl --stop` stop reading arg entry
    let tplEndEntry =
        ArgEntry cmdName
        |> _.addFlag(endFlag)

    /// turn bash-file's every single line which begins with `#> ...` to `echo "..."`
    /// this makes tab alignment possible
    let echoMapper (line : string) =
        let echoIdentifier = "#>"
        if line.TrimStart().StartsWith echoIdentifier then
            "echo \"" + line.TrimStart('#', '>').TrimEnd () + "\""
        else
            line
    /// body execution of `tpl` command
    let exeTpl : ReadonlyLinesBuffer -> ArgBehaviour = fun buffer dto ->
        buffer.all
        |> fun ctx -> evcms |> List.iter (fun m -> m.doMatch ctx)

        dto.parameters[bashFileParam].value<string>()
        |> ByFilePath
        |> LinesBuffer
        |> _.mapRest(echoMapper)
        |> _.executeInBash()
        |> _.Split("\n")
        |> Array.iter (fun l -> Out.appendLine l)

        revertEnvars()
    /// reading the source file context by lines
    let readByLines : ArgBehaviour = fun dto ->
        let mutable lineCount = dto.parameters[lineCountParam].value<int>()
        let buffer = LinesBuffer(ByNone)
        In.iterRest(fun raw ->
            let line = SourceLine(identifier, raw)
            match lineCount > 0 with
            | true when not line.isCmd -> // normal line encountered
                buffer.appendLine raw
                lineCount <- lineCount - 1
                ()
            | _ ->
                ()
        )

        exeTpl buffer dto // truly executes the `tpl` command
    /// reading the source file context by `tpl --start` to `tpl --end`
    let readByStartAndEnd : ArgBehaviour = fun dto ->
        let buffer = LinesBuffer(ByNone)
        let mutable reading = true
        In.iterRest(fun raw ->
            let line = SourceLine(identifier, raw)
            match reading with
            | true when not line.isCmd -> // normal line encountered
                buffer.appendLine raw
                ()
            | true when line.isCmd && tplEndEntry |> ArgEntry.validate line.args |> _.IsOk -> // `tpl --stop` encountered
                reading <- false
                ()
            | _ ->
                ()
        )

        exeTpl buffer dto // truly executes the `tpl` command


    CmdEntry(cmdName, cmdInfo)
    |> _.addEntry(ArgEntry(cmdName)
                  |> _.addParameter(lineCountParam)
                  |> _.addParameter(bashFileParam)
                  |> _.addBehaviour(readByLines)
    )
    |> _.addEntry(ArgEntry(cmdName)
                  |> _.addFlag(startFlag)
                  |> _.addParameter(bashFileParam)
                  |> _.addBehaviour(readByStartAndEnd)
    )
    |> _.addEntry(tplEndEntry)
