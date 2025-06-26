module Regl.CommandLine.Commands.GenCommand.UnsetEnvar

open System
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds
open Regl.CommandLine.Types.FlagsAndParams

let cmdName = "unset-envar"

let cmdInfo = "Unsets a environment variable"

let entry =
    let envarName = Param("envar-name", "the environment variable name")

    let exeUnsetEnvar : ArgBehaviour = fun dto ->
        let name = dto.parameters[envarName].value<string>()
        Environment.SetEnvironmentVariable(name, null)

    CmdEntry(cmdName, cmdInfo)
    |> _.addEntry(ArgEntry(cmdName)
                  |> _.addParameter(envarName)
                  |> _.addBehaviour(exeUnsetEnvar)
    )