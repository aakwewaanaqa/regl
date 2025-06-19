namespace Regl.CommandLine.Types

open System

type IFlag =
    /// The name of the flag
    abstract member name : string
    /// The explanation of the flag to be used like
    abstract member usage : string
    /// Indicates that this flag needs a following input
    abstract member needInput : bool
    /// Gets the value from raw string
    abstract member getVal : string -> FlagVal

    inherit IComparable