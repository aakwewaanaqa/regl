module Regl.CommandLine.Commands.GenCommand.SetEnvar

open System
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

///TODO : remove
[<Obsolete>]
let usage = "set-envar <ENVAR-NAME> <VALUE>
    Sets the environmental variable to a value"

///TODO : remove
[<Obsolete>]
let exe (r : CommandParseResult) =
    let varName = r.getParam 0
    let varValue = r.getParam 1
    Environment.SetEnvironmentVariable(varName, varValue)

///TODO : remove
[<Obsolete>]
let cmd =
    let builder = CommandBuilder("set-envar", exe)
    builder.parameters <- [ Param("envar-name"); Param("value") ]
    builder.build ()

//TODO : write entry