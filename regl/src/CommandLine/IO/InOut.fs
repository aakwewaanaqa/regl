module Regl.CommandLine.IO.InOut

open System
open System.IO

let mutable In : ReadonlyLinesBuffer = ReadonlyLinesBuffer(ByNone)

let mutable Out : LinesBuffer = LinesBuffer(ByNone)

let debugLog s =
    File.AppendAllText("Debug.log", $"{s}")