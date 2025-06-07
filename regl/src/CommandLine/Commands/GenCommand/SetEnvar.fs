module Regl.CommandLine.Commands.GenCommand.SetEnvar

open System
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage = "set-envar <ENVAR-NAME> <VALUE>
    Sets the environmental variable to a value"

let exe (result: ParseResult option) =
    match result with
    | Some result when result.parameters.Length >= 2 ->
        let varName = result.parameters[0]
        let varValue = result.parameters[1]
        Environment.SetEnvironmentVariable(varName, varValue)
    | Some _ ->
        raise (Exception usage)
    | None -> 
        raise (Exception usage)

let cmd =
    let builder = CommandBuilder("set-envar", exe)
    builder.requiredParamsCount <- 2
    builder.usage <- Some usage
    builder.build ()