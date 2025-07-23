namespace Fnm.Types

type Pattern(isExclusive : bool, nodes : INode list) =
    class
        member p.visit(cargo : PatternCargo) =
            let nc = NodeCargo(cargo.path, 0)
            let head = nodes.Head
            let result = head.visit nc
            match result with
                | Some _ ->
                    match isExclusive with
                    | true -> cargo.exclude() |> Some
                    | false -> cargo.``include``() |> Some
                | None ->
                    Some cargo
    end