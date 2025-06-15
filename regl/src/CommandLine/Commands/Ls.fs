module Regl.CommandLine.Commands.Ls

open System.IO
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.Builders

let usage = "regl ls [-R] [--pattern <PATTERN>]
    Lists fils in current's directory.
        -R        : Recursively searches the current directory.
        --pattern : Applies pattern to search method."

let exe (r: CommandParseResult) =
    let hasPattern = r.tryGetFlagValue "--pattern"
    let isRecursive = r.hasFlag "-R"
    let searchOption = ternary isRecursive SearchOption.AllDirectories SearchOption.TopDirectoryOnly
    let pattern = hasPattern |> FlagVal.defaultString ""

    Directory.GetCurrentDirectory()
    |> fun pwd -> Directory.GetFiles(pwd, pattern, searchOption)
    |> List.ofArray
    |> fun lines -> InOut.Out.lines <- lines

let cmd =

    let builder = CommandBuilder("ls", exe)
    builder.optionalFlags <- [ OnFlag("-R"); InStringFlag("--pattern") ]
    builder.usage <- usage
    builder.build ()
