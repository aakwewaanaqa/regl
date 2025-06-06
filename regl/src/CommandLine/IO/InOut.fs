module Regl.CommandLine.IO.InOut

open System

let mutable In : ReadonlyLinesBuffer = ReadonlyLinesBuffer(ByNone)

let mutable Out : LinesBuffer = LinesBuffer(ByNone)