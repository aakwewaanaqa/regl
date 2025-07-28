module Fnm.Helper.StringHelper

let findScope (opening : char) (closing : char) (str : string) =
    let mutable isEscaped = false
    let mutable foundStart = false
    let mutable startAt = 0
    let mutable endAt = str.Length - 1
    let characters = str.ToCharArray()
    
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