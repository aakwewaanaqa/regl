namespace Regl.CommandLine.Types

open System

type IFlag =
    abstract member name: string
    abstract member usage: string
    abstract member needInput : bool
    abstract member getVal : string -> FlagVal

    inherit IComparable

    inherit IEquatable<IFlag>