module Regl.CommandLine.Commands.GenCommand.Echo

open System
open Regl.CommandLine.Builders
open Regl.CommandLine.IO
open Regl.CommandLine.Types

///TODO : remove
[<Obsolete>]
let exe (r : CommandParseResult) =
    r.getParam 0
    |> InOut.Out.appendLine

///TODO : remove
[<Obsolete>]
let cmd : CommandBody =
    let builder = CommandBuilder("echo", exe)
    builder.parameters <- [ Param("text") ]
    builder.build()

//TODO : write entry