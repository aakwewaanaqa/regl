module Regl.CommandLine.Commands.GenCommand.UnsetEnvar

open System
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage = "//#!unset-envar <ENVAR-NAME>
    Unsets (removes) the specified environment variable"

let exe (r : CommandParseResult) =
    let varName = r.getParam 0
    Environment.SetEnvironmentVariable(varName, null)

let cmd =
    let builder = CommandBuilder("unset-envar", exe)
    builder.parameters <- [ Param("envar-name") ]
    builder.usage <- usage
    builder.build ()