module Regl.CommandLine.Commands.GenCommand.SetEnvar

open System
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    match result with
    | Some result when result.parameters.Length >= 2 ->
        let varName = result.parameters[0]
        let varValue = result.parameters[1]
        Environment.SetEnvironmentVariable(varName, varValue)
    | Some _ ->
        raise (Exception "set-envar requires two parameters: variable name and value")
    | None -> 
        raise (Exception "set-envar can't be executed...")

let cmd =
    let builder = CommandBuilder("set-envar", exe)
    builder.requiredParamsCount <- 2
    builder.usage <- Some "regl set-envar <VARIABLE_NAME> <VALUE>
    Sets an environment variable to the specified value"
    builder.build ()