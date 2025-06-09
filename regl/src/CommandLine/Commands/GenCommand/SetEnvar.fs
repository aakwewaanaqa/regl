module Regl.CommandLine.Commands.GenCommand.SetEnvar

open System
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage = "set-envar <ENVAR-NAME> <VALUE>
    Sets the environmental variable to a value"

let exe (r : CommandParseResult) =
    let varName = r.getParam 0
    let varValue = r.getParam 1
    Environment.SetEnvironmentVariable(varName, varValue)

let cmd =
    let builder = CommandBuilder("set-envar", exe)
    builder.parameters <- [ Param("envar-name"); Param("value") ]
    builder.build ()