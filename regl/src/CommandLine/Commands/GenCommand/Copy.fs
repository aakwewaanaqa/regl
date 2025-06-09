module Regl.CommandLine.Commands.GenCommand.Copy

open System
open Regl.CommandLine.Builders
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Commands.GenCommand.Shared

let exe (r: CommandParseResult) =
    for atLine in InOut.In.filterRest isNotCmd (r.getParamT<int> 0) do
        InOut.Out.appendLine atLine

let cmd =
    let builder = CommandBuilder("copy", exe)
    builder.parameters <- [ Param("<line-count>") ]
    builder.build()