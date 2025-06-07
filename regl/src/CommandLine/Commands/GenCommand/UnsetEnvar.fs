module Regl.CommandLine.Commands.GenCommand.UnsetEnvar

open System
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage = "//#!unset-envar <ENVAR-NAME>
    Unsets (removes) the specified environment variable"

let exe (result: ParseResult option) =
    match result with
    | Some result when result.parameters.Length >= 1 ->
        let varName = result.parameters[0]
        Environment.SetEnvironmentVariable(varName, null)
    | Some _ ->
        raise (Exception usage)
    | None -> 
        raise (Exception usage)

let cmd =
    let builder = CommandBuilder("unset-envar", exe)
    builder.requiredParamsCount <- 1
    builder.usage <- Some usage
    builder.build ()