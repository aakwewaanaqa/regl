namespace Regl.CommandLine.IO

open System
open System.IO
open System.Collections
open System.Collections.Generic

type BufferSource =
    | ByConsoleIn
    | ByFile of FileInfo
    | ByNone

and LinesBuffer(source: BufferSource) =
    member val _index: int = 0 with get, set
    member val _lines: string list =
        match source with
        | ByNone -> []
        | ByFile fileInfo -> fileInfo.OpenText().ReadToEnd().Split("\n") |> List.ofArray
        | ByConsoleIn -> Console.In.ReadToEnd().Split("\n") |> List.ofArray with get, set

    member this.Length = this._lines.Length

    member this.appendLine line = this._lines <- this._lines @ [ line ]

    member this.all() =
        this._lines |> List.reduce (fun a b -> $"{a}\n{b}")

    member this.rest(?count: int) =
        let startIndex = this._index

        let endIndex =
            match count with
            | Some count ->
                if this._index + count < this.Length then
                    this._index + count
                else
                    this.Length - 1
            | None -> this.Length - 1

        seq {
            for i in startIndex..endIndex do
                yield this._lines[i]
        }
