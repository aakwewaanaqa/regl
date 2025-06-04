module Regl.CommandLine.Commands.Gen.Copy

open System
open Regl.CommandLine.Shared
open Regl.CommandLine.Types
open Regl.CommandLine.Builders
open Regl.CommandLine.Types.Shared

let mutable lineCount : int = 0

let exe (result: ParseResult option) =
    lineCount <- getParam result 0 |> int

let cmd =
    let builder = CommandBuilder("copy", exe)
    builder.requiredParamsCount <- 1
    builder.build ()