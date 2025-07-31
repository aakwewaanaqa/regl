namespace Fnm.Pattern.Parse.Nodes.Bracket

open System
open Fnm.Helper
open Fnm.Pattern.Parse

type CharRange =
    struct
        val rangeStart: char
        val rangeEnd: char

        new(rangeStart: char, rangeEnd: char) =
            { rangeStart = rangeStart
              rangeEnd = rangeEnd }

    end

module CharRange =
    let tryParse: Parser<CharRange> =
        let fn cargo =
            let esMode = DecodeEscape
            let nmMode = Normal

            try
                let head0, rem = cargo |> StringCargo.tryHead esMode |> Option.get
                let head1, rem = rem |> StringCargo.tryHead nmMode |> Option.get

                if head1 = '-' then
                    let head2, rem = rem |> StringCargo.tryHead esMode |> Option.get
                    (CharRange(head0, head2), rem) |> Some
                else
                    None
            with _ ->
                None

        fn
