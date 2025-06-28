module Regl.CommandLine.Commands.GenCommand.Echo

open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds
open Regl.CommandLine.Types.FlagsAndParams

let cmdName = "echo"

let cmdInfo = "echos or writes to stdout"

let entry =

    let bodyParam = Param("body", "the body to write to stdout")

    let exeEcho : ArgBehaviour = fun dto ->
        dto.parameters[bodyParam].value<string>()
        |> Out.appendLine

    CmdEntry(cmdName)
        .addInfo(cmdInfo)
        .addEntry(ArgEntry()
            .addParameter(bodyParam)
            .addBehaviour(exeEcho))