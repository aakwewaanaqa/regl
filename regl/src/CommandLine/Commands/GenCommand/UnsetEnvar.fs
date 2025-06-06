module Regl.CommandLine.Commands.GenCommand.UnsetEnvar

open System
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    match result with
    | Some result when result.parameters.Length >= 1 ->
        let varName = result.parameters[0]
        Environment.SetEnvironmentVariable(varName, null)
    | Some _ ->
        raise (Exception "unset-envar requires one parameter: variable name")
    | None -> 
        raise (Exception "unset-envar can't be executed...")

let cmd =
    let builder = CommandBuilder("unset-envar", exe)
    builder.requiredParamsCount <- 1
    builder.usage <- Some "regl unset-envar <VARIABLE_NAME>
    Unsets (removes) the specified environment variable"
    builder.build ()