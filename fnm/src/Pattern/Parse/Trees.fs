module Fnm.Pattern.Parse.Trees

open System.Security.Principal
open Fnm.Helper
open Fnm.Pattern.Parse.Nodes

let orDo (second: Parser) (first: Parser): Parser =
    let combined cargo =
        match cargo |> first with
        | Some tuple -> Some tuple
        | None -> cargo |> second
    
    combined

/// we have to leave the character parser
/// that accepts any character to be matched at last
let basicParseTree =
    Negate.parseFn
    |> orDo Any.parseFn
    |> orDo WildCard.parseFn
    |> orDo Character.parseFn
