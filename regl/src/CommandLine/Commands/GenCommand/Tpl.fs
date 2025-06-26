module Regl.CommandLine.Commands.GenCommand.Tpl

open Regl.CommandLine.Commands.GenCommand.Types.Lines
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
    let lineCountParam =
        IntParam ("line-count", "reading lines with a specific count of line to stdout")

    let bashFileParam =
        Param("template-file", "the template files to echos stdout for generated file")

    let startFlag =
        BoolFlag("--start", "starts reading for source file context")

    let endFlag =
        BoolFlag("--end", "ends reading for source file context")

    let tplEndEntry =
        ArgEntry(cmdName, "starts tpl reading")
        |> _.addFlag(startFlag)

    let tplEndEntry =
        ArgEntry(cmdName, "stops tpl reading")
        |> _.addFlag(endFlag)

    let echoMapper (line : string) =
        let echoIdentifier = "#>"
        if line.TrimStart().StartsWith echoIdentifier then
            "echo \"" + line.TrimStart('#', '>').TrimEnd () + "\""
        else
            line

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

    let byLines : ArgBehaviour = fun dto ->
        let mutable lineCount = dto.parameters[lineCountParam].value<int>()
        let buffer = LinesBuffer(ByNone)
        In.iterRest(fun l ->
            let line = l |> Line
            if lineCount > 0 && line.isCmd |> not then
                buffer.appendLine l
                lineCount <- lineCount - 1
            )

        exeTpl buffer dto

    let byStartAndEnd : ArgBehaviour = fun dto ->
        let buffer = LinesBuffer(ByNone)
        let mutable reading = true
        In.iterRest(fun l ->
            let line = l |> Line
            if line.isCmd |> not && reading then
                buffer.appendLine l
            elif line.isCmd && line.args.Value[0] = "tpl" then
                let isEnd =
                    tplEndEntry
                    |> ArgEntry.validate line.args.Value.Tail
                    |> _.IsOk
                if isEnd then
                    reading <- false
            )

        exeTpl buffer dto


    CmdEntry(cmdName, cmdInfo)
    |> _.addEntry(ArgEntry(cmdName)
                  |> _.addParameter(lineCountParam)
                  |> _.addParameter(bashFileParam)
                  |> _.addBehaviour(byLines)
    )
    |> _.addEntry(ArgEntry(cmdName)
                  |> _.addFlag(startFlag)
                  |> _.addBehaviour(byStartAndEnd)
    )
