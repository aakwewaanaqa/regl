module Regl.CommandLine.Commands.Gen

open System.IO
open Regl.CommandLine.IO
open Regl.CommandLine.Builders
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.GenCommand
open Regl.CommandLine.Commands.GenCommand.Types
open Regl.CommandLine.Types.Utility

let mutable commandIdentifier: string = ""
let mutable evcms: EnvironmentVariableContextMatcher list = []

let isCmd (atLine: string) =
    atLine.TrimStart().StartsWith(commandIdentifier)

let isNotCmd (atLine: string) =
    not (atLine.TrimStart().StartsWith(commandIdentifier))

let exe (result: ParseResult option) =
    let genCmds = [| AddEvcm.cmd; Copy.cmd; Tpl.cmd |]
    let iter i (line : string) =
        if i = 0 then
            commandIdentifier <- line.TrimEnd()
        elif line.Trim().StartsWith(commandIdentifier) then
            let genArgv = line.Trim().Substring(commandIdentifier.Length).Split(" ")

            genCmds
            |> tryCommands genArgv
            |> function
                | 0 -> ()
                | n -> ()

    InOut.In.iteriRest iter

let cmd =
    let builder = CommandBuilder("gen", exe)
    builder.usage <- Some "regl gen"
    builder.build ()
