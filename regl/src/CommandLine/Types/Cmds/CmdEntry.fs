namespace Regl.CommandLine.Types.Cmds

open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.FlagsAndParams

type CmdEntry (name : string, ?info : string) =
    member c.name = name

    member val entries : ArgEntry list = [] with get, set

    member c.addEntry(entry : ArgEntry) =
        c.entries <- c.entries @ [ entry ]
        c

    override c.ToString() = name

module CmdEntry =
    let acceptCombos (combos : IFlag list list) (exe : ArgBehaviour) (cmd : CmdEntry) =
        combos
        |> List.map (fun combo ->
           cmd.addEntry(ArgEntry(cmd.name)
                        |> _.addFlags(combo)
                        |> _.addBehaviour(exe)))
        |> List.last