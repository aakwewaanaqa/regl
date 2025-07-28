namespace Fnm.Helper

type StringCargo =
    struct
        val str: string
        
        new(str: string) = { str = str }
        
        member cargo.Item
            with get (i: int) = cargo.str[i]

        member cargo.length
            with get () = cargo.str.Length
    end
    
module StringCargo =
    let tryHead (str : StringCargo) =
        if str.length > 0 then
            str[0] |> Some
        else
            None
            
    let trySubstring (src: StringCargo) (range: SubstringRange) =
        let isStartIn = range.startAt < src.length
        let isEndIn = range.endAt < src.length
        match range.endAt with
        | -1 when isStartIn ->
            src
            |> _.str.Substring(range.startAt)
            |> Some
        | endAt when isStartIn && isEndIn ->
            src
            |> _.str.Substring(range.startAt, endAt)
            |> Some
        | _ -> None