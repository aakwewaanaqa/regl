module Fnm.Pattern.Parse.Nodes.Character

open Fnm.Helper
open Fnm.Pattern.Parse

let private makeMatchFn (c: char): Matcher =
    let fn cargo =
        match cargo |> StringCargo.tryHead Normal with
        | Some(v, rem) when v = c -> Some rem 
        | _ -> None
        
    fn
    
let parseFn: Parser =
    let fn cargo =
        match cargo |> StringCargo.tryHead Escaping with
        | Some(c, rem) -> (makeMatchFn c, rem) |> Some
        | None -> None
    
    fn