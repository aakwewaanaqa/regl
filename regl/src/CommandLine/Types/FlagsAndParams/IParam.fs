namespace Regl.CommandLine.Types.FlagsAndParams

open System

type IParam =
    abstract member name : string
    abstract member usage : string
    abstract member getVal : string -> ArgVal

    inherit IComparable