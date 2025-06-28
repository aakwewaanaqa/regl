module Regl.CommandLine.Commands.GenCommand.AddEvcm

open System.Text.RegularExpressions
open Regl.CommandLine.Commands.GenCommand.Types
open Regl.CommandLine.Commands.GenCommand.Shared
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds
open Regl.CommandLine.Types.FlagsAndParams

let cmdName = "add-evcm"

let cmdInfo = "Adds an environment variable context matcher to tpl command"

let entry =
    let regexParam = RegexParam ("regex", "the pattern of matching tpl context")
    let formatParam = Param ("format", "the format of match to cast to")
    let envarParam = Param ("envar-name", "the name of the environment variable to declare with")

    let exeAddEvcm : ArgBehaviour =
        fun dto ->
            let pattern = dto.parameters[regexParam].value<Regex>()
            let format = dto.parameters[formatParam].value<string>()
            let envarName = dto.parameters[envarParam].value<string>()
            let newOne = EnvironmentVariableContextMatcher (pattern, format, envarName)
            evcms <- evcms @ [ newOne ]

    CmdEntry(cmdName)
        .addInfo(cmdInfo)
        .addEntry(ArgEntry()
            .addParameter(regexParam)
            .addParameter(formatParam)
            .addParameter(envarParam)
            .addBehaviour(exeAddEvcm))
