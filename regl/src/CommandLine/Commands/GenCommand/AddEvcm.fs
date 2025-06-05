module Regl.CommandLine.Commands.GenCommand.AddEvcm

open System.Text.RegularExpressions
open Regl.CommandLine.Builders
open Regl.CommandLine.Commands
open Regl.CommandLine.Commands.GenCommand.Types
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Shared


let exe (result: ParseResult option) : unit =
    let pattern = getParam result 0 |> Regex
    let format = getParam result 1
    let envarName = getParam result 2
    let newOne = EnvironmentVariableContextMatcher(pattern, format, envarName)
    Gen.evcms <- Gen.evcms @ [ newOne ]

let cmd: CommandBody =
    let builder = CommandBuilder("add-evcm", exe)
    builder.requiredParamsCount <- 3
    builder.build ()
