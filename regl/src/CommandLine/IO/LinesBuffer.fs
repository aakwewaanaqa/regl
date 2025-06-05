namespace Regl.CommandLine.IO

open System
open System.IO
open System.Collections
open System.Collections.Generic

type BufferSource =
    | ByNone
    | ByFile of FileInfo
    | BySeq of string seq
    | ByConsoleIn

and ReadonlyLinesBuffer(source: BufferSource) =
    member val _lines: string list = [] with get, set
    member val _index: int = 0 with get, set
    member this.all: string =
        match source with
        | ByNone -> ""
        | ByFile fileInfo ->
            let all = fileInfo.OpenText().ReadToEnd()
            this._lines <- all.Split("\n") |> List.ofArray
            all
        | BySeq sequence ->
            this._lines <- sequence |> List.ofSeq
            sequence |> Seq.reduce(fun a b -> $"{a}\n{b}")
        | ByConsoleIn ->
            let all = Console.In.ReadToEnd()
            this._lines <- all.Split("\n") |> List.ofArray
            all

    member this.length = this._lines.Length

    member this.filterRest (filter: string -> bool) (count: int) =
        let startIndex = this._index

        let endIndex =
            if this._index + count < this.length then
                this._index + count
            else
                this.length - 1

        seq {
            for i in startIndex..endIndex do
                let lineText = this._lines[i]

                if filter lineText then
                    yield lineText
        }

    member this.rest () =
        let startIndex = this._index
        let endIndex = this.length - 1

        seq {
            for i in startIndex..endIndex do
                yield this._lines[i]
        }

    member this.rest (count: int) =
        let startIndex = this._index

        let endIndex =
            if this._index + count < this.length then
                this._index + count
            else
                this.length - 1

        seq {
            for i in startIndex..endIndex do
                yield this._lines[i]
        }

and LinesBuffer(source: BufferSource) =
    inherit ReadonlyLinesBuffer(source)
    member this.appendLine line = this._lines <- this._lines @ [ line ]
