module Regl.CommandLine.Commands.Ls

open System
open System.IO
open Regl.CommandLine.IO
open TextCopy
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    match result with
    | Some _ ->

        let hasPattern = tryGetFlagValue result "--pattern"
        let isRecursive = hasFlag result "-R"
        let searchOption = ternary isRecursive SearchOption.AllDirectories SearchOption.TopDirectoryOnly
        let pattern = hasPattern |> Option.defaultValue ""

        Directory.GetCurrentDirectory()
        |> fun pwd -> Directory.GetFiles(pwd, pattern, searchOption)
        |> List.ofArray
        |> fun lines -> InOut.Out.lines <- lines

    | None -> raise (Exception "ls can't be executed...")

let cmd =

    let builder = CommandBuilder("ls", exe)
    builder.optionalFlags <- [ OnFlag("-R"); InStringFlag("--pattern") ]

    builder.usage <-
        Some
            "regl ls [-R] [--pattern <PATTERN>]
    List fils in current's directory.
    -R: Recursively search the current directory.
    --pattern: Apply pattern to search method."

    builder.build ()
