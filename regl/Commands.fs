module regl.Commands

open System.Text.RegularExpressions
open Builders.Source
open System
open System.IO
open TextCopy
open regl.Builders.Source

let mutable pIn =
    try
        Console.In.ReadToEnd()
    with _ ->
        ""

let copyCmd =
    let copyExe (result: ParseResult option) =
        match result with
        | Some result -> ClipboardService.SetText pIn
        | None -> raise (Exception "copy can't be executed...")

    CommandBuilder("copy", copyExe).build ()


let splitCmd =
    let splitExe (result: ParseResult option) =
        match result with
        | Some result ->
            pIn <-
                Regex(result.parameters[0])
                |> _.Split(pIn)
                |> Array.reduce (fun a b -> $"{a}\n{b}")
        | None -> raise (Exception "split can't be executed...")

    let builder = CommandBuilder("split", splitExe)
    builder.requiredParamsCount <- 1
    builder.build ()

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

            pIn <-
                let regex = Regex(result.parameters[0])
                let matches = regex.Matches(pIn)

                matches
                |> Seq.map (fun m ->
                    let mutable result = format

                    for i = 0 to m.Groups.Count - 1 do
                        result <- result.Replace($"${i}", m.Groups[i].Value)

                    result)
                |> Seq.reduce (fun a b -> $"{a}\n{b}")
        | None -> raise (Exception "match can't be executed...")

    let builder = CommandBuilder("match", matchExe)
    builder.requiredParamsCount <- 1
    builder.optionalFlags <- [ InString("--format") ]
    builder.build ()

let removeEmptyCmd =
    let removeEmptyExe (result: ParseResult option) =
        match result with
        | Some _ ->
            pIn <-
                pIn
                |> _.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                |> Array.reduce (fun a b -> $"{a}\n{b}")
        | None -> raise (Exception "removeEmpty can't be executed...")

    let builder = CommandBuilder("remove-empty", removeEmptyExe)
    builder.build ()

let toFileCmd =
    let toFileExe (result: ParseResult option) =
        match result with
        | Some result ->
            let path = result.parameters[0]
            File.WriteAllText(path, pIn)
        | None -> raise (Exception "to-file can't be executed...")

    let builder = CommandBuilder("to-file", toFileExe)
    builder.requiredParamsCount <- 1
    builder.build ()
