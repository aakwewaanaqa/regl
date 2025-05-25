// For more information see https://aka.ms/fsharp-console-apps
namespace Regl

open System.IO
open System.Text
open System.Text.RegularExpressions

module Program =

    open System
    open Argu
    open TextCopy

    type Commands =
        | [<AltCommandLine("copy")>] Copy
        | [<AltCommandLine("split")>] Split of string
        | [<AltCommandLine("match")>] Match of MatchOption

        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | Copy -> "Copies txt to clipboard."
                | Split _ -> "Separates txt to lines."
                | Match _ -> "Matches txt as lines and returns passed lines"

    and MatchOption =
        { Pattern: string
          [<AltCommandLine("-f")>]
          Format: string option }

    let (?>) (flag: bool) (ifTrue, ifFalse) = if flag then ifTrue else ifFalse

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
                        match cmd with
                        | Copy -> ClipboardService.SetText pipeIn
                        | Split dlmt -> // dlmt as delimiter
                            pipeIn <- pipeIn |> _.Split(dlmt) |> Array.reduce (fun a b -> $"{a}\n{b}")
                        | Match opts -> // ptrn as pattern
                            let sb = StringBuilder()
                            let lines = pipeIn.Replace("\r", "").Split("\n")
                            let regex = (opts.Pattern |> String.IsNullOrEmpty) ?> (".*", opts.Pattern) |> Regex

                            for line in lines do
                                let m = regex.Match line

                                if m.Success then
                                    if opts.Format.IsSome then
                                        let mutable buffer = opts.Format.Value
                                        m.Groups |> Seq.iteri (fun i g -> buffer <- buffer.Replace($"${i}", g.Value))
                                    else
                                        sb.AppendLine m.Value |> ignore
                                else
                                    ()

                            pipeIn <- sb.ToString()

                    Console.Write(pipeIn)
                with :? ArguParseException as ex ->
                    printfn $"%s{ex.Message}"

                0

        rt
