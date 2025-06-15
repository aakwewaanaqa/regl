namespace Regl.CommandLine.Types

open System

type IFlag =
    abstract member name: string
    abstract member usage: string
    abstract member hasVal : bool
    abstract member getVal : string

    inherit IComparable