namespace fnm.Functions

open fnm.Types

type AChar(c : char) =
    member val next : INode option = None
    
    interface INode with
        override w.next : INode option = w.next

        override w.visit (f : Flux) : Flux option =
            if f.head = c then
                let rem = f |> Flux.take 1
                match w.next with
                | Some next -> rem |> next.visit
                | None -> rem |> Some
            else
                None