module Regl.CommandLine.Commands.Ls

open System
open System.IO
open TextCopy
open Regl.CommandLine.Shared
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
    match result with
    | Some _ ->

        let hasPattern = tryGetFlagValue result "--pattern"
        let isRecursive = hasFlag result "-R"

        let searchOption =
            if isRecursive then
                SearchOption.AllDirectories
            else
                SearchOption.TopDirectoryOnly

        let pattern = hasPattern |> Option.defaultValue ""

        let out =
            Directory.GetCurrentDirectory()
            |> (fun pwd -> Directory.GetFiles(pwd, pattern, searchOption))
            |> Array.reduce (fun a b -> $"{a}\n{b}")

        writeOut out
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
