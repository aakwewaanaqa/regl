namespace Regl.CommandLine.IO

open System
open System.Diagnostics
open System.IO
open Regl.Lang

type BufferSource =
    | ByNone
    | ByFile of FileInfo
    | ByFilePath of string
    | BySeq of string seq
    | ByList of string list
    | ByConsoleIn

and ReadonlyLinesBuffer (source : BufferSource) =
    let _all =
        match source with
        | ByNone -> ""
        | ByFile fileInfo -> fileInfo.OpenText().ReadToEnd ()
        | ByFilePath path -> File.ReadAllText (path)
        | BySeq sequence -> sequence |> Seq.reduce (fun a b -> $"{a}\n{b}")
        | ByList list -> list |> List.reduce (fun a b -> $"{a}\n{b}")
        | ByConsoleIn -> Console.In.ReadToEnd ()

    let _lines = _all.Split ("\n") |> List.ofArray

    abstract member all : string with get, set

    default this.all
        with get () = _all
        and set _ = raise (InvalidOperationException "Hey! No touchy! This is read-only! 🙈")

    abstract member lines : string list with get, set

    default this.lines
        with get () = _lines
        and set _ = raise (InvalidOperationException "Hey! No touchy! This is read-only! 🙈")

    member val index : int = 0 with get, set

    member this.length = this.lines.Length


    member this.reset() = this.index <- 0

    member this.rest() =
        let startIndex = this.index
        let endIndex = this.length - 1

        seq {
            for i in startIndex..endIndex do
                yield this.lines[i]
        }

    member this.iteriRest(iter : int -> string -> unit) =
        let startIndex = this.index
        let endIndex = (this.length - 1)

        for i in startIndex..endIndex do
            if i < this.length then
                iter i this.lines[i]

    member this.filterRest (filter : string -> bool) (count : int) =
        let startIndex = this.index

        let endIndex =
            if this.index + count < this.length then
                this.index + count
            else
                this.length - 1

        seq {
            for i in startIndex..endIndex do
                let lineText = this.lines[i]

                if filter lineText then
                    yield lineText
        }

    member buffer.executeInBash() =
        let tmpShName = "tmp.sh"
        File.WriteAllText ("tmp.sh", buffer.all)

        let startInfo = ProcessStartInfo ()
        startInfo.FileName <- "/bin/bash"
        startInfo.Arguments <- tmpShName
        startInfo.RedirectStandardOutput <- true
        startInfo.UseShellExecute <- false
        let prcs = Process.Start startInfo
        prcs.WaitForExit ()

        if prcs.ExitCode > 0 then
            raise (ExceptionLang.bashCrash prcs.ExitCode)
        else
            prcs.StandardOutput.ReadToEnd ()

and LinesBuffer (source) =
    inherit ReadonlyLinesBuffer (source)

    member val private _lines : string list =
        match source with
        | ByNone -> []
        | ByFile fileInfo -> fileInfo.OpenText().ReadToEnd().Split ("\n") |> List.ofArray
        | ByFilePath path -> File.ReadAllText(path).Split ("\n") |> List.ofArray
        | BySeq sequence -> sequence |> List.ofSeq
        | ByList list -> list
        | ByConsoleIn -> Console.In.ReadToEnd().Split ("\n") |> List.ofArray with get, set

    override this.lines
        with get () = this._lines
        and set v = this._lines <- v

    override this.all
        with get () = this.lines |> List.reduce (fun a b -> $"{a}\n{b}")
        and set v = this.lines <- v.Split ("\n") |> List.ofArray

    member this.appendLine line = this.lines <- this.lines @ [ line ]

    member this.mapRest(mapper : string -> string) =
        let startIndex = this.index
        let rest = this.lines |> List.skip startIndex
        let mapped = rest |> List.map mapper
        this.lines <- (this.lines |> List.take startIndex) @ mapped
        this

    member this.sendToPipe() = Console.Out.WriteLine this.all
