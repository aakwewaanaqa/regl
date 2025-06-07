module Regl.CommandLine.Commands.GenCommand.Echo

open System
open Regl.CommandLine.Builders
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared

let exe (result : ParseResult option) =
    match result with
    | Some r ->
        r.getParam 0
        |> InOut.Out.appendLine
    | None -> raise (NotImplementedException "needs echo usage here...")

let cmd : CommandBody =
    let builder = CommandBuilder("echo", exe)
    builder.requiredParamsCount <- 1
    builder.build()