module Regl.CommandLine.Commands.Split

open System
open System.Text.RegularExpressions
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO
open TextCopy
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    InOut.In <- ReadonlyLinesBuffer(ByConsoleIn)
    match result with
    | Some _ ->
        Regex(getParam result 0)
        |> _.Split(InOut.In.all)
        |> List.ofArray
        |> fun lines -> InOut.Out.lines <- lines
    | None -> raise (Exception "split can't be executed...")

let cmd =

    let builder = CommandBuilder("split", exe)
    builder.requiredParamsCount <- 1
    builder.usage <- Some "regl split <DELIMITER>"
    builder.build ()
