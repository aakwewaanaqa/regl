namespace fnm.Types

type INode =
    abstract member visit : Flux -> Flux option
    abstract member next : INode option