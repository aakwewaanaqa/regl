module Fnm.Pattern.Parse.Nodes.Character

open Fnm.Helper
open Fnm.Pattern.Parse

let private makeMatchFn (character: char): Matcher =
    let fn cargo =
        match cargo |> StringCargo.tryHead Normal with
        | Some(c, rem) when c.character = character -> Some rem 
        | _ -> None
        
    fn |> CannotFail
    
let parseFn: Parser =
    let fn cargo =
        match cargo |> StringCargo.tryHead Escaping with
        | Some(c, rem) -> (makeMatchFn c.character, rem) |> Some
        | None -> None
    
    fn