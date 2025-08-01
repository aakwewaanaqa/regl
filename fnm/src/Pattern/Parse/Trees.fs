module Fnm.Pattern.Parse.Trees

open System
open Fnm.Helper
open Fnm.Pattern.Parse.Nodes


let basicParse (pattern: StringCargo) =
    let bindParser (second: Parser) (first: Parser) : Parser =
        let combined cargo =
            match cargo |> first with
            | Some tuple -> Some tuple
            | None -> cargo |> second

        combined

    /// This is the parse tree of a fnm basic pattern,
    /// consists of '*?' and any character.
    /// We have to leave the character parser
    /// that accepts any character to be matched at last.
    let parseTree =
        Any.parseFn
        |> bindParser WildCard.parseFn
        |> bindParser Character.parseFn

    /// Matchers have to be bound for the condition that if the first passed. 
    let bindMatcher (second: Matcher) (first: Matcher) : Matcher =
        let combined cargo =
            let rec run () =
                match cargo |> first.func with
                | Some rem ->
                    match rem |> second.func with
                    | Some rem -> Some rem
                    | None when first.IsCanRetry -> run ()
                    | None -> None
                | None -> None
            
            run ()

        combined |> CannotFail

    let rec matchTree (previous: Matcher) (rem: StringCargo) =
        match rem |> parseTree with
        | Some(next, rem) ->
            let combined = previous |> bindMatcher next

            if rem.length > 0 then
                rem |> matchTree combined
            else
                previous |> Some
        | None -> None

    let passed: Matcher =
        let fn cargo = cargo |> Some

        fn |> CannotFail
        
    matchTree passed pattern