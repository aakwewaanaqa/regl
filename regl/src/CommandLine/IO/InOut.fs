module Regl.CommandLine.IO.InOut

open System

let mutable In : LinesBuffer = LinesBuffer(ByNone)

let mutable Out : LinesBuffer = LinesBuffer(ByNone)

let readFromPipe () =
    In <- LinesBuffer(ByConsoleIn)

let writeToPipe () =
    Out._lines
    |> List.iter Console.Out.WriteLine