module Regl.CommandLine.Commands.ToFile

open System.IO
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage = "regl to-file <FILE-PATH> [--append]
    Writes piped input to a file
        --append : Appends writing
"

let exe (r: CommandParseResult) =
    // Reads piped input
    InOut.In <- ReadonlyLinesBuffer(ByConsoleIn)

    let path = r.getParam 0

    if r.hasFlag "--append" then
        File.AppendAllText(path, InOut.In.all)
    else
        File.WriteAllText(path, InOut.In.all)

let cmd =
    let builder = CommandBuilder("to-file", exe)
    builder.usage <- usage
    builder.parameters <- [ Param("<FILE>") ]
    builder.build ()
