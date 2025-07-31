namespace Fnm.Pattern.Parse.Nodes

open Fnm.Helper
open Fnm.Pattern.Parse

type WildCard =
    struct        
        interface IPatternParseNode<WildCard> with
            override n.tryParse cargo =
                try
                    match cargo |> StringCargo.tryHead Normal with
                    | Some (head, rem) when head = '*' ->
                        Some (WildCard(), rem)
                    | _ ->
                        None
                with _ ->
                    None
    end