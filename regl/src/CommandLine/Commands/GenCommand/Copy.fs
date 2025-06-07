module Regl.CommandLine.Commands.GenCommand.Copy

open System
open Regl.CommandLine.Builders
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Commands.GenCommand.Shared

let exe (result: ParseResult option) =
    match result with
    | Some r ->
        for atLine in InOut.In.filterRest isNotCmd (r.getParamT<int> 0) do
            InOut.Out.appendLine atLine
    | None -> raise (NotImplementedException "need copy usage here...")

let cmd =
    let builder = CommandBuilder("copy", exe)
    builder.requiredParamsCount <- 1
    builder.build()