namespace Fnm.Helper

open System.Text


type ConsumeMode =
    | Escaping
    | Normal

type StringCargo =
    struct
        val str: string
        
        new(str: string) = { str = str }
        
        member cargo.Item
            with get (i: int) = cargo.str[i]

        member cargo.length
            with get () = cargo.str.Length
            
        member cargo.take(count: int) =
            cargo.str.Substring(count - 1) |> StringCargo
    end
    
module StringCargo =
    let tryHead (mode: ConsumeMode) (cargo : StringCargo) =
        if cargo.length < 1 then
            None
        elif mode = Escaping && cargo[0] = '\\' then
            if cargo.length < 2 then
                None
            else    
                (cargo[1], cargo.take 2) |> Some
        else
            (cargo[0], cargo.take 1) |> Some

    let trySubstring (range: SubstringRange) (src: StringCargo) =
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
        
    let tryTake (count: int) (mode: ConsumeMode) (cargo: StringCargo) =
        if cargo.length < count then
            None
        elif mode = Escaping then
            let mutable isEscaped = false
            let mutable builder = StringBuilder()
            
            let rec loopFn (chars: char list) =
                if builder.Length = count then
                    builder.ToString() |> Some
                elif chars.Length = 0 then
                    None
                else
                    match chars.Head with
                    | '\\' when not isEscaped ->
                        isEscaped <- true
                        chars.Tail |> loopFn
                    | c ->
                        isEscaped <- false
                        builder <- builder.Append c
                        chars.Tail |> loopFn
            
            cargo.str.ToCharArray()
            |> List.ofArray
            |> loopFn
            |> function
                | Some value ->            
                    let rem = cargo.str.Substring(value.Length) |> StringCargo
                    (value, rem) |> Some
                | None ->
                    None
        else
            let value = cargo.str.Substring(0, count)
            let rem   = cargo.str.Substring(count) |> StringCargo
            (value, rem) |> Some
            
    let tryFindScope (opening: char) (closing: char) (mode: ConsumeMode) (cargo: StringCargo) =
        let mutable isEscaped = false
        let mutable foundStart = false
        let mutable startAt = 0
        let mutable endAt = cargo.length - 1
        let characters = cargo.str.ToCharArray()
        
        characters
        |> Array.iteri (fun i c ->
            match c with
            | '\\' when not isEscaped && mode = Escaping ->
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
            | '\\' when not isEscaped && mode = Escaping ->
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
            