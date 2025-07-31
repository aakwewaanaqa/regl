namespace Fnm.Pattern.Parse.Nodes

open Fnm.Helper
open Fnm.Pattern.Parse

type Negate =
    struct        
        interface IPatternParseNode<Negate> with
            override n.tryParse cargo =
                try
                    match cargo |> StringCargo.tryHead Normal with
                    | Some (head, rem) when head = '!' ->
                        Some (Negate(), rem)
                    | _ ->
                        None
                with _ ->
                    None
    end