namespace Regl.CommandLine.Types

type ICommandBuilder =
    abstract member name: string
    abstract member usage: string
    abstract member requiredParamCount: int
    abstract member optionalFlags: IFlag list
    abstract member requiredFlags: IFlag list

type ICommandBuilder<'a> =
    inherit ICommandBuilder
    abstract member build: unit -> 'a
