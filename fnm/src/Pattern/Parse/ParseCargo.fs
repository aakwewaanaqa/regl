namespace Fnm.Pattern.Parse

open Fnm.Helper

type ParseCargo =
    struct
        val private _str: string

        new(str: string) = { _str = str }

        member c.rem = c._str
        member c.head = c.rem[0]
        member c.length = c._str.Length
        member c.isDepleted = c.length = 0
    end

module ParseCargo =
    let tryTake (count: int) (cargo: ParseCargo) =
        match cargo.length with
        | 0 -> None
        | length when length >= count ->
            cargo.rem.Substring(0, count)
            |> ParseCargo
            |> Some
        | _ -> None
        
    let tryTakeChar (chara: char) (cargo: ParseCargo) =
        match cargo.head with
        | c when c = chara ->
            cargo.rem.Substring 1
            |> ParseCargo
            |> Some
        | _ -> None

    let tryTakeScope (opening: char) (closing: char) (cargo: ParseCargo) =
        cargo.rem
        |> StringHelper.findScope opening closing
        |> function
            | Some(startAt, endAt) ->
                cargo.rem
                |> _.Substring(startAt, endAt - startAt + 1)
                |> ParseCargo
                |> Some
            | None -> None
