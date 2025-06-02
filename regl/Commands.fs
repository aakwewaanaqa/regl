module regl.Commands

open System.Text.RegularExpressions
open Builders.Source
open System
open TextCopy

let mutable pIn =
    try
        Console.In.ReadToEnd()
    with _ ->
        ""

let copyExe (result: ParseResult option) =
    match result with
    | Some result -> ClipboardService.SetText pIn
    | None -> raise (Exception "Copy can't be executed...")

let copyCmd = CommandBuilder("copy", copyExe).build ()

let splitExe (result: ParseResult option) =
    match result with
    | Some result ->
        pIn <-
            Regex(result.parameters[0])
            |> _.Split(pIn)
            |> Array.reduce (fun a b -> $"{a}\n{b}")
    | None -> raise (Exception "split can't be executed...")

let splitCmd =
    let splitBuilder = CommandBuilder("split", splitExe)
    splitBuilder.requiredParamsCount <- 1
    splitBuilder.build ()

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

let matchCmd =
    let matchBuilder = CommandBuilder("match", matchExe)
    matchBuilder.requiredParamsCount <- 1
    matchBuilder.optionalFlags <- [ InString("--format") ]
    matchBuilder.build ()
