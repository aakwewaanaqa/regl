module Regl.CommandLine.Commands.Match

open System
open System.Text.RegularExpressions
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage = "regl match <REGEX> [--format <FORMAT>]"

let exe (result: ParseResult option) =
    InOut.In <- ReadonlyLinesBuffer(ByConsoleIn)
    match result with
    | Some v when v.parameters.Length >= 1 ->

        let pattern = getParam result 0

        let format = tryGetFlagValue result "--format" |> Option.defaultValue "$0"

        pattern
        |> Regex
        |> _.Matches(InOut.In.all)
        |> Seq.map (fun m -> formatMatch m format)
        |> List.ofSeq
        |> fun lines -> InOut.Out.lines <- lines
    | Some _ -> raise (Exception usage)
    | None -> raise (Exception usage)

let cmd =
    let builder = CommandBuilder("match", exe)
    builder.requiredParamsCount <- 1
    builder.optionalFlags <- [ InStringFlag("--format") ]
    builder.usage <- Some usage
    builder.build ()
