module Fnm.Types.Builders.MatchBuilder

open System
open System.IO
open Fnm.Types

let private lineFilter (line : string) =
    let line = line.Trim()
    if String.IsNullOrEmpty line then
        false
    elif String.IsNullOrWhiteSpace line then
        false
    elif line.StartsWith('#') then
        false
    else
        true

let private option =
    StringSplitOptions.RemoveEmptyEntries |||
    StringSplitOptions.TrimEntries

let ofFilePath (path : string) =
    FileInfo path
    |> _.OpenText()
    |> _.ReadToEnd()
    |> _.Split('\n', option)
    |> Array.filter lineFilter
    |> Array.map PatternBuilder.compile
    |> List.ofArray
    |> Matcher

let ofRaw (raw : string) =
    raw
    |> _.Split('\n', option)
    |> Array.filter lineFilter
    |> Array.map PatternBuilder.compile
    |> List.ofArray
    |> Matcher    