namespace Fnm.Nodes

open Fnm.Types

type AChar(c: char) =
    member val next: INode option = None with get, set

    interface INode with
        override a.next: INode option = a.next

        override a.visit(f: NodeCargo) : NodeCargo option =
            if f |> NodeCargo.isDepleted then
                None
            elif f.head = c then
                let rem = f |> NodeCargo.take 1

                match a.next with
                | Some next -> rem |> next.visit
                | None -> rem |> Some
            else
                None
                
        override a.setNext(next: INode) =
            a.next <- next |> Some
            a