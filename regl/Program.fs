// For more information see https://aka.ms/fsharp-console-apps
namespace Regl

open System.IO

module Program =

    open System
    open Argu
    open TextCopy

    type Commands =
        | [<AltCommandLine("copy")>] Copy
        | [<AltCommandLine("split")>] Split of string

        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | Copy -> "Copies txt to clipboard."
                | Split _ -> "Separates txt to lines."

    type Arguments =
        | [<AltCommandLine("-e")>] Pattern of string

        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | Pattern _ -> "To gives pattern of Regex..."

    [<EntryPoint>]
    let Main args =
        let mutable pipeIn = Console.In.ReadToEnd()

        let rt =
            if pipeIn |> String.IsNullOrEmpty then
                printfn "regl must be used after a pipe to input txt..."
                1
            else
                let parser = ArgumentParser.Create<Commands>()

                try
                    let results = parser.ParseCommandLine args

                    for cmd in results |> _.GetAllResults() do
                        Console.WriteLine $"==> {cmd}"
                        match cmd with
                        | Copy ->
                            ClipboardService.SetText pipeIn
                        | Split separator ->
                            pipeIn <- pipeIn |> _.Split(separator) |> Array.reduce (fun a b -> $"{a}\n{b}")

                    Console.Out.Write(pipeIn)
                with :? ArguParseException as ex ->
                    printfn $"%s{ex.Message}"

                0

        rt
