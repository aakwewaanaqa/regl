module Regl.CommandLine.Commands.GenCommand.Implementation

open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Commands.GenCommand
open Regl.CommandLine.Commands.GenCommand.Shared

let cmds = [|
    AddEvcm.cmd
    Copy.cmd
    SetEnvar.cmd
    Tpl.cmd
    UnsetEnvar.cmd
|]

let exe (result: ParseResult option) =
    let iteri i (line : string) =
        if i = 0 then
            identifier <- line.TrimEnd()
        elif line.Trim().StartsWith(identifier) then
            let genArgv = line.Trim().Substring(identifier.Length).Split(" ")

            cmds
            |> tryCommands genArgv
            |> function
                | 0 -> ()
                | n -> ()

    InOut.In.iteriRest iteri
