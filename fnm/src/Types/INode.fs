namespace Fnm.Types

type INode =
    abstract member visit : NodeCargo -> NodeCargo option
    abstract member next : INode option
    abstract member setNext : INode -> INode
