module Regl.CommandLine.Commands.GenCommand.SetEnvar

open System
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds
open Regl.CommandLine.Types.FlagsAndParams

let cmdName = "set-envar"

let cmdInfo = "sets environment variable to an appointed value"

let entry =
    let envarNameParam = Param("envar-name", "the name of the environment variable to be set")

    let envarValueParam = Param("envar-val", "the value of the environment variable to be set with")

    let exeSetEnvar : ArgBehaviour = fun dto ->
        let name = dto.parameters[envarNameParam].value<string>()
        let value = dto.parameters[envarValueParam].value<string>()
        Environment.SetEnvironmentVariable(name, value)

    CmdEntry(cmdName, cmdInfo)
    |> _.addEntry(ArgEntry(cmdName)
                  |> _.addParameter(envarNameParam)
                  |> _.addParameter(envarValueParam)
                  |> _.addBehaviour(exeSetEnvar)
    )
