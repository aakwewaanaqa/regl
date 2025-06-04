module Regl.CommandLine.Shared

open System

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
