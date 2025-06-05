module Regl.CommandLine.Commands.GenCommand.Copy

open Regl.CommandLine.Builders
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Commands.GenCommand.Shared

let exe (result: ParseResult option) =
    for atLine in In.filterRest isNotCmd (getParam result 0 |> int) do
        Out.appendLine atLine

let cmd =
    let builder = CommandBuilder("copy", exe)
    builder.requiredParamsCount <- 1
    builder.build()