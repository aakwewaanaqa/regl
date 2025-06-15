module Regl.CommandLine.Commands.Match

open System.Text.RegularExpressions
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let usage = "regl match <REGEX> [--format <FORMAT>]"

let exe (r: CommandParseResult) =
    InOut.In <- ReadonlyLinesBuffer(ByConsoleIn)
    let pattern = r.getParam 0
    let format = r.tryGetFlagValue "--format" |> FlagVal.defaultString "$0"

    pattern
    |> Regex
    |> _.Matches(InOut.In.all)
    |> Seq.map (fun m -> formatMatch m format)
    |> List.ofSeq
    |> fun lines -> InOut.Out.lines <- lines

let cmd =
    let builder = CommandBuilder("match", exe)
    builder.parameters <- [ Param("<REGEX>") ]
    builder.optionalFlags <- [ InStringFlag("--format") ]
    builder.usage <- usage
    builder.build ()
