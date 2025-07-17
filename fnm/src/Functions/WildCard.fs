namespace fnm.Functions

open fnm.Types

type WildCard() =
    member val next: INode option = None

    interface INode with
        override w.next: INode option = w.next

        override w.visit(f: Flux) : Flux option =
            match w.next with
            | Some next ->
                let rec loopStep (count: int) =
                    f
                    |> Flux.take count
                    |> next.visit
                    |> function
                        | Some f -> Some f
                        | None -> loopStep (count + 1)

                loopStep 0
            | None -> Flux(f.src, f.src.Length - 1) |> Some
