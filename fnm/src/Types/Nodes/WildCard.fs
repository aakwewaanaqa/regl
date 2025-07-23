namespace Fnm.Nodes

open Fnm.Types

type WildCard() =
    member val next: INode option = None with get, set

    interface INode with
        override w.next: INode option = w.next

        override w.visit(f: NodeCargo) : NodeCargo option =
            match w.next with
            | Some next ->
                let rec loopStep (count: int) =
                    f
                    |> NodeCargo.take count
                    |> next.visit
                    |> function
                        | Some f -> Some f
                        | None -> loopStep (count + 1)

                loopStep 0
            | None -> NodeCargo(f.src, f.src.Length - 1) |> Some

        override a.setNext(next: INode) =
            a.next <- next |> Some
            a