module Regl.Exts

open System
open System.Collections.Generic

let guard (b : bool) (ex : string) =
    if b then raise (Exception ex)
    else ()

let guardResult (r : Result<'a, 'b>) =
    match r with
    | Ok v -> v
    | Error v -> raise (Exception $"{v}")

let guardTry (ex : string) (r : 'a option) =
    match r with
    | Some s -> s
    | None -> raise (Exception ex)

let inline (|>?) arg (flag : bool, func) =
    if flag then func arg
    else arg

let inline (|>??) (arg : 'a) (flag : bool, onTrue : 'a -> 'b, onFalse : 'a -> 'b) =
    if flag then onTrue arg
    else onFalse arg

let inline (<-??) (flag : bool) (okVal, noVal) =
    if flag then okVal
    else noVal

let inline (/??) (dict : Dictionary<'a, 'b>) (key : 'a, def : 'b) =
    if dict.ContainsKey key then
        dict[key]
    else
        def