module Fnm.Pattern.Parse.Nodes.Bracket.CharRange

open Fnm.Helper
open Fnm.Pattern.Parse

let private makeMatchFn (fromChar: char) (toChar: char): Matcher =
    let fn cargo =
        match cargo |> StringCargo.tryHead Normal with
        | Some(c, rem) ->
            let c = c.character |> int
            let f = fromChar |> int
            let t = toChar |> int
            if c >= f && c <= t then Some rem else None
        | None -> None

    fn |> CannotFail

let parseFn: Parser =
    let fn cargo =
        try
            let fromChar, rem = cargo |> StringCargo.tryHead Escaping |> Option.get
            let hyphen, rem = rem |> StringCargo.tryHead Escaping |> Option.get

            match hyphen with
            | NotEscaped c when c = '-' -> 
                let toChar, rem = rem |> StringCargo.tryHead Escaping |> Option.get
                (makeMatchFn fromChar.character toChar.character, rem) |> Some
            | _ ->
                None
        with _ ->
            None

    fn
