namespace Fnm.Types

type Pattern =
    struct
        val private _isExclusive : bool
        val private _nodes : INode list
        
        new (isExclusive : bool, nodes : INode list) = {
            _isExclusive = isExclusive
            _nodes = nodes
        }
        
        member p.visit(cargo : PatternCargo) =
            let nc = NodeCargo(cargo.path, 0)
            let head = p._nodes.Head
            let result = head.visit nc
            match result with
                | Some _ ->
                    match p._isExclusive with
                    | true -> cargo.exclude()
                    | false -> cargo.``include``()
                | None ->
                    cargo
    end