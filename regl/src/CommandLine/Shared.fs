module Regl.CommandLine.Shared

open System
open System.Text.RegularExpressions

/// This function takes a string as an argument and directly writes it to the standard output stream of the Console.
let writeOut (a: string) = Console.Out.Write(a)

/// This function takes a string argument and writes it to the standard output stream of the console, appending a new line at the end.
let writeOutLine (a: string) = Console.Out.WriteLine(a)

let throwIfNone (a: 'a option) (msg: string) =
    if a.IsNone then raise (Exception(msg)) else ()

let setEnvar key value =
    Environment.SetEnvironmentVariable(key, value)

let getEnvar key = Environment.GetEnvironmentVariable(key)

let hasEnvar key =
    not (Environment.GetEnvironmentVariable(key) |> String.IsNullOrEmpty)

let formatMatch (m: Match) (format: string) =
    let mutable formatted = format
    m.Groups
    |> Seq.iteri (fun i g ->
        if g.Success then
            formatted <- formatted.Replace($"${i}", g.Value)
        )
    formatted