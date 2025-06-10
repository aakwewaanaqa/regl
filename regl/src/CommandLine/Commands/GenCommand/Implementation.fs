module Regl.CommandLine.Commands.GenCommand.Implementation

open System
open Regl.CommandLine.IO
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types
open Regl.CommandLine.Builders
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Commands.GenCommand
open Regl.CommandLine.Commands.GenCommand.Shared

let subCmds = [
    AddEvcm.cmd
    Copy.cmd
    Echo.cmd
    Import.cmd
    SetEnvar.cmd
    Tpl.cmd
    UnsetEnvar.cmd
]

let exe (r : CommandParseResult) =
    let iteri i (line: string) =
        In.index <- i

        if i = 0 then
            identifier <- line.TrimEnd()
        elif line.Trim().StartsWith(identifier) then
            line
            |> _.Trim()
            |> _.Substring(identifier.Length)
            |> parseCommandLineArgs
            |> tryCommands subCmds
            |> function
                | Ok () -> ()
                | Error ex -> debugLog $"regl gen -> {ex}"

    r.tryGetFlagValue "--file"
    |> function
        | Some path -> In <- ReadonlyLinesBuffer(ByFilePath (path.ToString()))
        | None -> In <- ReadonlyLinesBuffer(ByConsoleIn)

    In.iteriRest iteri

let cmd =
    let builder = CommandBuilder("gen", exe)
    builder.optionalFlags <- [ InStringFlag("--file") ]
    builder.build ()
