module Regl.CommandLine.Commands.GenCommand.Echo

open System
open Regl.CommandLine.Builders
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared

let exe (r : CommandParseResult) =
    r.getParam 0
    |> InOut.Out.appendLine

let cmd : CommandBody =
    let builder = CommandBuilder("echo", exe)
    builder.parameters <- [ Param("text") ]
    builder.build()