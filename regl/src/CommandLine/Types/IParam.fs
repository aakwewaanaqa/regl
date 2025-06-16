namespace Regl.CommandLine.Types

open System

type IParam =
    abstract member name : string
    abstract member usage : string
    abstract member parse : string -> string
    abstract member getVal : string -> FlagVal
    inherit IComparable