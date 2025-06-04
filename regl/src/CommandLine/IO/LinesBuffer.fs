namespace Regl.CommandLine.IO

open System
open System.IO

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

    member this.appendLine line =
        this._lines <- this._lines @ [ line ]

    member this.createSubBuffer() =
        let sub = LinesBuffer(ByNone)
        sub._index <- this._index
        sub._lines <- this._lines
        sub
