module Fnm.Pattern.Parse.Nodes.WildCard

open Fnm.Helper
open Fnm.Pattern.Parse

let private matchFn: Matcher =
    let mutable attempt = 0

    let fn cargo =
        match cargo |> StringCargo.tryTake attempt Normal with
        | Some(_, rem) ->
            attempt <- attempt + 1
            rem |> Some
        | None -> None

    fn

let parseFn: Parser =
    let fn cargo =
        match cargo |> StringCargo.tryHead Normal with
        | Some(head, rem) when head = '*' -> (matchFn, rem) |> Some
        | _ -> None

    fn
