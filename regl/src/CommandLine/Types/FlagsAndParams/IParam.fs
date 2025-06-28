namespace Regl.CommandLine.Types.FlagsAndParams

open System

type IParam =
    abstract member name : string
    abstract member info : string
    abstract member getVal : string -> ArgVal

    inherit IComparable