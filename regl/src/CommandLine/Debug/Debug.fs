module Regl.CommandLine.Debug

open System
open System.Diagnostics
open System.IO

let private logPath = "debug.log"

let private isConsole =
       not Console.IsOutputRedirected  
    && not (Environment.GetCommandLineArgs().[0] = "gen")

let private logFile =
    match isConsole with
    | true -> null
    | false -> File.AppendText(logPath)

/// the writer for debugging issues
type private Writer(isError : bool) =        
    let writer : TextWriter =
        match isConsole with
        | false ->
            logFile
        | true ->
            match isError with
            | true -> Console.Error
            | false -> Console.Out
    
    member val private _bgColor = ConsoleColor.Black with get, set
    member val private _fgColor = ConsoleColor.White with get, set
    
    member private w.bgColor 
        with get () =
            match isConsole with
            | true -> Console.BackgroundColor
            | false -> w._bgColor
        and set (c : ConsoleColor) =
            let correspondingColor =
                match c with
                | ConsoleColor.Blue
                | ConsoleColor.Red
                | ConsoleColor.Green
                | ConsoleColor.White -> ConsoleColor.Black
                | ConsoleColor.Black -> ConsoleColor.White
                | _ -> ConsoleColor.White
            match isConsole with
            | true ->
                Console.BackgroundColor <- c
                Console.ForegroundColor <- correspondingColor
            | false ->
                w._bgColor <- c
                w._fgColor <- correspondingColor
    
    /// to write with bg color
    /// and name it with capital C for tuple input
    member w.Write (msg : string, ?bgColor : ConsoleColor) =
        let bgColor = defaultArg bgColor ConsoleColor.Black
        
        w.Write(DateTime.Now.ToString())
        match isError with
        | true ->
            w.bgColor <- ConsoleColor.Red
            writer.Write(" ERR  ")
            w.bgColor <- ConsoleColor.Black
        | false ->
            w.bgColor <- ConsoleColor.Green
            w.Write(" INFO ")
            w.bgColor <- ConsoleColor.Black
            
        w.bgColor <- bgColor
        writer.Write msg
        w.bgColor <- ConsoleColor.Black
        writer.Flush()
        
let private logWriter =
    Writer false

let writeLog a =
    logWriter.Write($"{a}\n")

let private errWriter =
    Writer true

let writeErr error =
    errWriter.Write($"{error}\n")

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

let close() =
    match isConsole with
    | true -> ()
    | false ->
        logFile.Flush()
        logFile.Close()