module Regl.CommandLine.Debug

open System
open System.Diagnostics
open System.IO

let private logPath = "debug.log"

let private toDebugLog =
    Console.IsOutputRedirected 
    || Environment.GetCommandLineArgs().[0] = "gen"

let private logWriter: TextWriter =
    if toDebugLog then
        File.AppendText(logPath)
    else
        Console.Out

let writeLog a =
    logWriter.Write(DateTime.Now.ToString())
    logWriter.Write(" INFO ")
    logWriter.Write($"{a}\n")
    logWriter.Flush()

let private errWriter: TextWriter =
    if toDebugLog then
        File.AppendText(logPath)
    else
        Console.Error

let writeErr error =
    errWriter.Write(DateTime.Now.ToString())
    errWriter.Write(" ERROR ")
    errWriter.Write($"{error}\n")
    errWriter.Flush()
    errWriter.Close()

let close () =
    logWriter.Close()
    errWriter.Close()

let through a =
    let trace = StackTrace(true)

    writeLog "Start processing stack trace"

    for i in 0 .. trace.FrameCount - 1 do
        let frame = trace.GetFrame i
        let method = frame.GetMethod()

        let fileName =
            match frame.GetFileName() with
            | null -> "Unknown"
            | f -> f

        let lineNumber = frame.GetFileLineNumber()

        writeLog (
            "Stack Frame [{FrameNumber}] Method: {MethodName} at {FileName}:{LineNumber}",
            i,
            method.Name,
            fileName,
            lineNumber
        )

    a
