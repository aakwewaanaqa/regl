namespace Regl.CommandLine.Types

open System
open Regl.CommandLine.Types.FlagsAndParams

module Flags =
    let rec combinations k lst =
        match k, lst with
        | 0, _ -> [[]]
        | _, [] -> []
        | k, x::xs ->
            let useX = combinations (k-1) xs |> List.map (fun combo -> x::combo)
            let skipX = combinations k xs
            useX @ skipX

    let getAllPossibilities (src : IFlag list) =
        let n = List.length src - 1
        seq {
            for k in 1 .. n + 1 do
                yield! combinations k [0..n]
                |> Seq.map (fun indices ->
                    indices |> List.map (fun i -> List.item i src))
        } |> List.ofSeq
