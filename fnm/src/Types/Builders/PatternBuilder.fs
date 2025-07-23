module Fnm.Types.Builders.PatternBuilder

open Fnm.Types
open Fnm.Nodes

let compile (raw: string) : Pattern =
    let raw = raw.Trim()
    let isExclusive = raw.StartsWith('!') |> not
    let raw = raw.TrimStart('!')
    let mutable nodes: INode list = []

    for c in raw do
        match c with
        | '*' -> nodes <- nodes @ [ WildCard() ]
        | c -> nodes <- nodes @ [ AChar(c) ]

    let rec connectNodes (input: INode list) =
        match input with
        | [] -> ()
        | [ _ ] -> ()
        | head :: tail ->
            head.setNext tail.Head |> ignore
            connectNodes tail

    connectNodes nodes

    Pattern(isExclusive, nodes)
