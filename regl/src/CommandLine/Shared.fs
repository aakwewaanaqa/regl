module Regl.CommandLine.Shared

open Regl.CommandLine.Commands
open Regl.CommandLine.Types.Cmds

let cmds =
    [| Copy.entry
       LexFix.entry
       Ls.entry
       Match.entry
       RemoveEmpty.entry
       Split.entry
       ToFile.entry
       GenCommand.Implementation.entry |]
    
let manual =
    cmds
    |> Array.map CmdEntry.getManual
    |> Array.reduce (fun a b -> $"{a}\n\n{b}")
    |> fun m -> "\n" + m
    