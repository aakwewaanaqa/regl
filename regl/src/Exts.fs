module Regl.Exts

open System

let guard (b : bool) (ex : string) =
    if b then raise (Exception ex)
    else ()

let guardResult (r : Result<'a, 'b>) =
    match r with
    | Ok v -> v
    | Error v -> raise (Exception $"{v}")