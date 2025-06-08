namespace Regl.CommandLine.Types

type IParam =
    abstract member name : string
    abstract member usage : string
    abstract member parse : string -> string