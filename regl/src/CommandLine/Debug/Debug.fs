module Regl.CommandLine.Debug

open System.Diagnostics
open System.IO

let through a =
    let trace = StackTrace(true)
    for i in 0..trace.FrameCount - 1 do
        let frame = trace.GetFrame i
        File.AppendAllText ("debug.log", $"{frame.GetMethod()} -> \n" )
    File.AppendAllText ("debug.log", $"-> {a}\n" )
    a