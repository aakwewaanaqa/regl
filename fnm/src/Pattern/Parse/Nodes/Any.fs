module Fnm.Pattern.Parse.Nodes.Any

open Fnm.Helper
open Fnm.Pattern.Parse

let matchFn: Matcher =
    let fn cargo =
        match cargo |> StringCargo.tryHead Normal with
        | Some(c, rem) -> rem |> Some
        | None -> None
    fn

let parseFn: Parser =
    let fn cargo =
        match cargo |> StringCargo.tryHead Normal with
        | Some(c, rem) when c = '?' -> (matchFn, rem) |> Some
        | _ -> None
    fn