module Regl.CommandLine.Commands.GenCommand.SetEnvar

open System
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage = "set-envar <ENVAR-NAME> <VALUE>
    Sets the environmental variable to a value"

let exe (result: ParseResult option) =
    match result with
    | Some r ->
        let varName = r.getParam 0
        let varValue = r.getParam 1
        Environment.SetEnvironmentVariable(varName, varValue)
    | None -> 
        raise (Exception usage)

let cmd =
    let builder = CommandBuilder("set-envar", exe)
    builder.requiredParamsCount <- 2
    builder.usage <- Some usage
    builder.build ()