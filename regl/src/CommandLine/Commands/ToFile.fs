module Regl.CommandLine.Commands.ToFile

open System
open System.IO
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds

///TODO : remove
[<Obsolete>]
let usage =
    "regl to-file <FILE-PATH> [--append]
    Writes piped input to a file
        --append : Appends writing
"

///TODO : remove
[<Obsolete>]
let exe (r : CommandParseResult) =
    // Reads piped input
    InOut.In <- ReadonlyLinesBuffer (ByConsoleIn)

    let path = r.getParam 0

    if r.hasFlag "--append" then
        File.AppendAllText (path, InOut.In.all)
    else
        File.WriteAllText (path, InOut.In.all)

///TODO : remove
[<Obsolete>]
let cmd =
    let builder = CommandBuilder ("to-file", exe)
    builder.usage <- usage
    builder.parameters <- [ Param ("<FILE>") ]
    builder.build ()

///TODO : use me
let entry =
    let write : ArgBehaviour =
        fun dto ->
            InOut.In <- ReadonlyLinesBuffer (ByConsoleIn)
            let path = dto.parameters[Param "<file-path>"].ToString ()
            File.WriteAllText (path, InOut.In.all)

    let appendWrite : ArgBehaviour =
        fun dto ->
            InOut.In <- ReadonlyLinesBuffer (ByConsoleIn)
            let path = dto.parameters[Param "<file-path>"].ToString ()
            File.AppendAllText (path, InOut.In.all)

    CmdEntry ("to-file", "write stdin to a file")
    |> _.addEntry(
        ArgEntry "write new file"
        |> _.addParameter(Param ("file-path", "file to write to"))
        |> _.addBehaviour(write)
    )
    |> _.addEntry(
        ArgEntry "append old file"
        |> _.addParameter(Param ("file-path", "file to write to"))
        |> _.addFlag(OnFlag ("--append", "writing will append to end of the file"))
        |> _.addBehaviour(appendWrite)
    )
