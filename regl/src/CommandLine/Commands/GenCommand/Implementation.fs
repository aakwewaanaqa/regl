module Regl.CommandLine.Commands.GenCommand.Implementation

open System
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Commands.GenCommand
open Regl.CommandLine.Commands.GenCommand.Shared

let subCmds = [| AddEvcm.cmd; Copy.cmd; SetEnvar.cmd; Tpl.cmd; UnsetEnvar.cmd |]

let exe (result: ParseResult option) =
    let iteri i (line: string) =
        InOut.In.index <- i

        if i = 0 then
            identifier <- line.TrimEnd()
        elif line.Trim().StartsWith(identifier) then
            let genArgv = line.Trim().Substring(identifier.Length).Split(" ")

            subCmds
            |> tryCommands genArgv
            |> function
                | 0 -> ()
                | n -> ()

    tryGetFlagValue result "--file"
    |> function
        | Some path -> InOut.In <- ReadonlyLinesBuffer(ByFilePath path)
        | None ->
            InOut.In <- ReadonlyLinesBuffer(ByConsoleIn)

    InOut.In.iteriRest iteri

let cmd =
    let builder = CommandBuilder("gen", exe)
    builder.usage <- Some "regl gen"
    builder.optionalFlags <- [ InStringFlag("--file") ]
    builder.build ()
