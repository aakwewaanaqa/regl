module Regl.CommandLine.IO.LinesReader

open System
open System.Text

let mutable index = -1
let mutable lines = [||]

let setFromIn () =
    lines <- Console.In.ReadToEnd() |> _.Split("\n")

let readLines (advance: bool) (count: int) : string =
    let builder = StringBuilder()

    let rec read (advance: bool) (count: int) : string =
        let adv = index + 1

        if adv < lines.Length then
            builder.AppendLine lines[adv] |> ignore
            if advance then index <- adv else ()
            read advance (count - 1)
        else
            builder.ToString()

    read advance count

let allLines () =
    lines |> Array.reduce (fun a b -> $"{a}\n{b}")
