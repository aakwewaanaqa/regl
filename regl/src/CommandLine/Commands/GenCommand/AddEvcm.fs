module Regl.CommandLine.Commands.GenCommand.AddEvcm

open System
open System.Text.RegularExpressions
open Regl.Lang
open Regl.CommandLine.Types
open Regl.CommandLine.Builders
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Commands.GenCommand.Types
open Regl.CommandLine.Commands.GenCommand.Shared

let exe (result : ParseResult option) : unit =
    match result with
    | Some r ->
        let pattern = r.getParam 0 |> Regex
        let format = r.getParam 1
        let envarName = r.getParam 2
        let newOne = EnvironmentVariableContextMatcher (pattern, format, envarName)
        evcms <- evcms @ [ newOne ]
    | None -> raise(Exception addEvcmUsage)

let cmd : CommandBody =
    let builder = CommandBuilder ("add-evcm", exe)
    builder.requiredParamsCount <- 3
    builder.build ()
