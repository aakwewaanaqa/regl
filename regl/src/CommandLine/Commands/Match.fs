module Regl.CommandLine.Commands.Match

open System
open System.Text.RegularExpressions
open Regl.CommandLine.IO
open Regl.CommandLine.Shared
open Regl.CommandLine.Types
open Regl.CommandLine.Builders

let exe (result: ParseResult option) =
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
            let matches = regex.Matches(InOut.In.all)

            matches
            |> Seq.map (fun m ->
                let mutable result = format

                for i = 0 to m.Groups.Count - 1 do
                    result <- result.Replace($"${i}", m.Groups[i].Value)

                result)
            |> Seq.reduce (fun a b -> $"{a}\n{b}")

        writeOut out
    | None -> raise (Exception "match can't be executed...")

let cmd =
    let builder = CommandBuilder("match", exe)
    builder.requiredParamsCount <- 1
    builder.optionalFlags <- [ InStringFlag("--format") ]
    builder.usage <- Some "regl match <REGEX> [--format <FORMAT>]"
    builder.build ()
