module Regl.CommandLine.Commands.GenCommand.Tpl

open Regl.CommandLine.Builders
open Regl.CommandLine.Commands
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Shared

let exe (result: ParseResult option) =
    InOut.In.filterRest Gen.isNotCmd (getParam result 0 |> int)
    |> fun sequence -> ReadonlyLinesBuffer(BySeq sequence)
    
let cmd =
    let builder = CommandBuilder("tpl", exe)
    builder.requiredParamsCount <- 1
    builder.build()