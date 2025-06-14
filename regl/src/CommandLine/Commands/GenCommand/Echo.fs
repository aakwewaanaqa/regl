module Regl.CommandLine.Commands.GenCommand.Echo

open Regl.CommandLine.Builders
open Regl.CommandLine.IO
open Regl.CommandLine.Types

let exe (r : CommandParseResult) =
    r.getParam 0
    |> InOut.Out.appendLine

let cmd : CommandBody =
    let builder = CommandBuilder("echo", exe)
    builder.parameters <- [ Param("text") ]
    builder.build()