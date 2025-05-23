// For more information see https://aka.ms/fsharp-console-apps
module Program

open System
open Argu

type Arguments =
    | [<AltCommandLine("-e")>] Pattern of string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Pattern _ -> "To gives pattern of Regex..."

[<EntryPoint>]
let main args =

    let pipe = Console.In.ReadToEnd()
    printfn $"{pipe}"
    
    let parser = ArgumentParser.Create<Arguments>()
    try
        let results = parser.ParseCommandLine args
        ()
    with
    | :? ArguParseException as ex ->
        // 印出 Argu 自動產生的使用說明
        printfn "%s" ex.Message

    0
