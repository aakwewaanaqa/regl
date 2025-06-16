namespace Regl.CommandLine.Types

open System

type IFlag =
    /// The name of the flag
    abstract member name : string
    /// The explanation of the flag to be used like
    abstract member usage : string

    abstract member needInput : bool

    abstract member getVal : string -> FlagVal

    inherit IComparable
