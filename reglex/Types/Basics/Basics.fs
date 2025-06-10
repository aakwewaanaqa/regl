module reglex.Types.Basics

open System
open System.Text.RegularExpressions

let pLetter (str : string) =
    Regex("[a-zA-Z]+").IsMatch(str)

let pDigit (str : string) =
    Regex("[0-9]+").IsMatch(str)

let pCostume (pattern : Regex) (str : string) =
    pattern.IsMatch(str)

let pString (str : string) (parsers : (char -> bool) list) =
    let rec parse (current : char list) =
        match current with
        | head :: tail ->
            parsers
            |> List.tryFind (fun p -> p head)
            |> function
                | Some _ -> parse tail
                | None -> false
        | [] -> true
    parse (str.ToCharArray() |> List.ofArray)