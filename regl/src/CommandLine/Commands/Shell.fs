/// <summary>
/// This is the module to deal with common commands from the command line.
/// </summary>
/// <remarks>
///     The following commands are supported:
///     - copy: Copy piped input to clipboard
///     - split: Split input text using specified delimiter
///     - match: Match text using regex with optional output format
///     - remove-empty: Remove empty lines from piped input
///     - ls: List files in the current directory with optional recursive search
///     - to-file: Write input content to a specified file with optional append mode
///     - gen: Generic command for generating content based on piped in a source file and commands inside the file
/// </remarks>
module Regl.CommandLine.Commands

open System
open System.IO
open System.Text.RegularExpressions
open TextCopy
open Regl.CommandLine.Shared
open Regl.CommandLine.Builders
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Shared

/// Split input text using specified delimiter
let splitCmd =
    let splitExe (result: ParseResult option) =
        match result with
        | Some result ->
            let out =
                Regex(result.parameters[0])
                |> _.Split(readIn ())
                |> Array.reduce (fun a b -> $"{a}\n{b}")

            writeOut out
        | None -> raise (Exception "split can't be executed...")

    let builder = CommandBuilder("split", splitExe)
    builder.requiredParamsCount <- 1
    builder.usage <- Some "regl split <DELIMITER>"
    builder.build ()

/// Match text using regex with optional output format
let matchCmd =
    let matchExe (result: ParseResult option) =
        match result with
        | Some result ->
            let format =
                result.flags
                |> Array.tryFind (fun arg -> arg.name = "--format")
                |> Option.map (fun f -> f :?> IInFlag<string>)
                |> Option.bind (fun f -> Some f.value)
                |> Option.defaultValue "$0"

            let out =
                let regex = Regex(result.parameters[0])
                let matches = regex.Matches(readIn ())

                matches
                |> Seq.map (fun m ->
                    let mutable result = format

                    for i = 0 to m.Groups.Count - 1 do
                        result <- result.Replace($"${i}", m.Groups[i].Value)

                    result)
                |> Seq.reduce (fun a b -> $"{a}\n{b}")

            writeOut out
        | None -> raise (Exception "match can't be executed...")

    let builder = CommandBuilder("match", matchExe)
    builder.requiredParamsCount <- 1
    builder.optionalFlags <- [ InString("--format") ]
    builder.usage <- Some "regl match <REGEX> [--format <FORMAT>]"
    builder.build ()

/// Remove empty lines from piped input
let removeEmptyCmd =
    let removeEmptyExe (result: ParseResult option) =
        match result with
        | Some _ ->
            let out =
                readIn ()
                |> _.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                |> Array.reduce (fun a b -> $"{a}\n{b}")

            writeOut out
        | None -> raise (Exception "removeEmpty can't be executed...")

    let builder = CommandBuilder("remove-empty", removeEmptyExe)
    builder.usage <- Some "regl remove-empty"
    builder.build ()

/// List files in the current directory with optional recursive search
let lsCmd =
    let lsExe (result: ParseResult option) =

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

    let builder = CommandBuilder("ls", lsExe)
    builder.optionalFlags <- [ OnFlag("-R"); InString("--pattern") ]

    builder.usage <-
        Some
            "regl ls [-R] [--pattern <PATTERN>]
    List files directory from the current directory.
    -R: Recursively search the current directory.
    --pattern: Apply pattern to search method."

    builder.build ()

/// Write input content to a specified file with optional append mode
let toFileCmd =
    let toFileExe (result: ParseResult option) =
        match result with
        | Some result ->
            let isAppend =
                result.flags |> Array.tryFind (fun f -> f.name = "--append") |> _.IsSome

            let path = result.parameters[0]

            if isAppend then
                File.AppendAllText(path, readIn ())
            else
                File.WriteAllText(path, readIn ())
        | None -> raise (Exception "to-file can't be executed...")

    let builder = CommandBuilder("to-file", toFileExe)
    builder.usage <- Some "regl to-file [--append] <FILEPATH>"
    builder.requiredParamsCount <- 1
    builder.build ()

/// Generic command for generating content based on specific format
let genCmd =
    let genExe (result: ParseResult option) =

        let mutable beginning = ""
        let mutable copyLineCount = 0

        let copyLineCmd =
            let copyLineExe (result: ParseResult option) =
                match result with
                | Some result -> copyLineCount <- result.parameters[0] |> int
                | None -> ()

            let builder = CommandBuilder("copy", copyLineExe)
            builder.requiredParamsCount <- 1
            builder.build ()

        let genCmds = [ copyLineCmd ]

        readIn ()
        |> _.Split("\n")
        |> Array.iteri (fun i line ->

            if i = 0 then
                beginning <- line
            elif line.Trim().StartsWith(beginning) then
                let genCmd = line.Trim().Substring(beginning.Length)
                let genArgv = genCmd.Split(" ")

                match genCmds |> List.tryFind (fun c -> c.parse (genArgv) |> _.IsSome) with
                | Some cmd -> cmd.parse genArgv |> cmd.execute
                | None -> ()
            else if copyLineCount > 0 then
                writeOutLine line
                copyLineCount <- copyLineCount - 1
            else
                ()

        )

    let builder = CommandBuilder("gen", genExe)
    builder.usage <- Some "regl gen"
    builder.build ()

let cmds = [ copyCmd; splitCmd; matchCmd; removeEmptyCmd; toFileCmd; lsCmd; genCmd ]
