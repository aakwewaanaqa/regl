module Fnm.Pattern.Parse.Nodes.Negate

open Fnm.Helper
open Fnm.Pattern.Parse

let private matchFn: Matcher =
    let fn cargo =
        match cargo |> StringCargo.tryHead Normal with
        | Some(head, rem) when head = '!' -> Some rem
        | _ -> None

    fn

let parseFn: Parser =
    let fn cargo =
        match cargo |> StringCargo.tryHead Normal with
        | Some(head, rem) when head = '!' -> (matchFn, rem) |> Some
        | _ -> None

    fn
