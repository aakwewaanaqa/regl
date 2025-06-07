module Regl.CommandLine.Commands.Ls

open System
open System.IO
open Regl.CommandLine.IO
open TextCopy
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Builders

let usage = "regl ls [-R] [--pattern <PATTERN>]
    Lists fils in current's directory.
        -R        : Recursively searches the current directory.
        --pattern : Applies pattern to search method."

let exe (result: ParseResult option) =
    match result with
    | Some r ->
        let hasPattern = r.tryGetFlagValue"--pattern"
        let isRecursive = r.hasFlag "-R"
        let searchOption = ternary isRecursive SearchOption.AllDirectories SearchOption.TopDirectoryOnly
        let pattern = hasPattern |> Option.defaultValue ""

        Directory.GetCurrentDirectory()
        |> fun pwd -> Directory.GetFiles(pwd, pattern, searchOption)
        |> List.ofArray
        |> fun lines -> InOut.Out.lines <- lines

    | None -> raise (Exception usage)

let cmd =

    let builder = CommandBuilder("ls", exe)
    builder.optionalFlags <- [ OnFlag("-R"); InStringFlag("--pattern") ]
    builder.usage <- Some usage
    builder.build ()
