module Regl.CommandLine.Debug

open System
open System.Diagnostics
open System.IO

let private logPath = "debug.log"

let logWriter: TextWriter =
    if Console.IsOutputRedirected then
        File.AppendText(logPath)
    else
        Console.Out

let writeLog a =
    logWriter.Write(DateTime.Now.ToString("o"))
    logWriter.Write(" INFO ")
    logWriter.Write($"{a}\n")
    logWriter.Flush()

let errWriter: TextWriter =
    if Console.IsErrorRedirected then
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

    // 使用結構化日誌
    writeLog "Start processing stack trace"

    for i in 0 .. trace.FrameCount - 1 do
        let frame = trace.GetFrame i
        let method = frame.GetMethod()

        let fileName =
            match frame.GetFileName() with
            | null -> "Unknown"
            | f -> f

        let lineNumber = frame.GetFileLineNumber()

        // 使用結構化日誌格式
        writeLog (
            "Stack Frame [{FrameNumber}] Method: {MethodName} at {FileName}:{LineNumber}",
            i,
            method.Name,
            fileName,
            lineNumber
        )

    a
