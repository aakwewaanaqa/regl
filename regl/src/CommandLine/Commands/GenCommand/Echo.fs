module Regl.CommandLine.Commands.GenCommand.Echo

open Regl.CommandLine.Builders
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared

let exe (result : ParseResult option) =
    getParam result 0
    |> InOut.Out.appendLine

let cmd : CommandBody =
    let builder = CommandBuilder("echo", exe)
    builder.requiredParamsCount <- 1
    builder.build()