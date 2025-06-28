module Regl.CommandLine.Commands.Split

open System
open System.Text.RegularExpressions
open Regl.CommandLine.Types.FlagsAndParams
open Regl.Exts
open Regl.CommandLine.IO
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds

let cmdName = "split"

let cmdInfo = "splits stdin to lines and writes to stdout"

//TODO : write entry
let entry =

    let paramDelimiter = Param ("delimiter", "the regex pattern to split with")
    let flagQuote = BoolFlag ("--quote", "to quote with \"")
    let flagTrim = BoolFlag ("--trim", "to trim leading and following spaces...")

    let exeSplit : ArgBehaviour =
        fun dto ->
            let hasTrim = dto.flags.containsFlag flagTrim
            let hasQuote = dto.flags.containsFlag flagQuote
            let regex = dto.parameters[paramDelimiter] |> _.ToString() |> Regex

            In <- ReadonlyLinesBuffer ByStdIn
            regex.Split In.all
            |> List.ofArray
            |>? (hasTrim, List.map _.Trim())
            |> List.filter (fun l -> l |> String.IsNullOrEmpty |> not)
            |>? (hasQuote, List.map (fun l -> $"\"{l}\""))
            |> fun lines -> Out.lines <- lines

    CmdEntry(cmdName)
        .addInfo(cmdInfo)
        .addEntry(ArgEntry()
             .addParameter(paramDelimiter)
             .addBehaviour(exeSplit))
        .addEntry(ArgEntry()
             .addParameter(paramDelimiter)
             .addFlag(flagQuote)
             .addBehaviour(exeSplit))
        .addEntry(ArgEntry()
             .addParameter(paramDelimiter)
             .addFlag(flagTrim)
             .addBehaviour(exeSplit))
        .addEntry(ArgEntry()
             .addParameter(paramDelimiter)
             .addFlag(flagQuote)
             .addFlag(flagTrim)
             .addBehaviour(exeSplit))
