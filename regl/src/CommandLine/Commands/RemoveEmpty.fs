module Regl.CommandLine.Commands.RemoveEmpty

open System
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: CommandParseResult) =
    InOut.In <- ReadonlyLinesBuffer(ByConsoleIn)
    InOut.In.lines
    |> List.filter (fun l -> not (l |> String.IsNullOrEmpty))
    |> List.iter (fun l -> InOut.Out.appendLine l)

    InOut.Out.sendToPipe()

let cmd =

    let builder = CommandBuilder("remove-empty", exe)
    builder.usage <- "regl remove-empty
    Execute the remove-empty command
    "
    builder.build ()
