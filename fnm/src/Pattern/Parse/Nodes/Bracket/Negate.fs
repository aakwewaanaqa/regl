module Fnm.Pattern.Parse.Nodes.Bracket.Negate

open Fnm.Helper
open Fnm.Pattern.Parse

let private matchFn: Matcher =
    let fn cargo =
        match cargo |> StringCargo.tryHead Normal with
        | Some(c, rem) when c.character = '!' -> Some rem
        | _ -> None

    fn |> CannotFail

let parseFn: Parser =
    let fn cargo =
        match cargo |> StringCargo.tryHead Escaping with
        | Some(c, rem) ->
            match c with
            | NotEscaped c when c = '!' -> (matchFn, rem) |> Some
            | _ -> None
        | _ -> None

    fn
