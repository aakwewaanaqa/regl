namespace Fnm.Helper


type CargoMode =
    | DecodeEscape
    | Normal

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
        
    let tryScope (opening: char) (closing: char) (mode: CargoMode) (cargo: StringCargo) =
        let mutable isEscaped = false
        let mutable foundStart = false
        let mutable startAt = 0
        let mutable endAt = cargo.length - 1
        let characters = cargo.str.ToCharArray()
        
        characters
        |> Array.iteri (fun i c ->
            match c with
            | '\\' when not isEscaped ->
                isEscaped <- true
            | '\\' when isEscaped ->
                isEscaped <- false
            | c when not foundStart && not isEscaped && c = opening ->
                startAt <- i
                foundStart <- true
            | _ -> ()
        )
        
        characters
        |> Array.iteri (fun i c ->
            match c with
            | '\\' when not isEscaped ->
                isEscaped <- true
            | '\\' when isEscaped ->
                isEscaped <- false
            | c when not isEscaped && c = closing ->
                endAt <- i
            | _ -> ()
        )
        
        if foundStart then
            SubstringRange(startAt, endAt - startAt + 1) |> Some
        else
            None