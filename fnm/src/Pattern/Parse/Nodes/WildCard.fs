module Fnm.Pattern.Parse.Nodes.WildCard

open Fnm.Helper
open Fnm.Pattern.Parse

let private makeMatchFn (): Matcher =
    let mutable attempt = 0

    let fn cargo =
        match cargo |> StringCargo.tryTake attempt Normal with
        | Some(_, rem) ->
            attempt <- attempt + 1
            rem |> Some
        | None -> None

    fn |> CanRetry

let parseFn: Parser =
    let fn cargo =
        match cargo |> StringCargo.tryHead Escaping with
        | Some(c, rem) ->
            match c with
            | NotEscaped c when c = '*' -> (makeMatchFn (), rem) |> Some
            | _ -> None
        | _ -> None

    fn
