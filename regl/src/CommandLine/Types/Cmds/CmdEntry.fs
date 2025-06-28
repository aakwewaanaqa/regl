namespace Regl.CommandLine.Types.Cmds

open System.Text
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.FlagsAndParams

type CmdEntry (name : string, ?info : string) =
    member c.name = name

    member c.info = info |> Option.defaultValue ""
    
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
        
    let getManual (cmd : CmdEntry) =
        StringBuilder()
        |> _.AppendLine($"Command:")
        |> _.AppendLine($"    regl {cmd.name}")
        |> _.AppendLine($"        {cmd.info}")
        |> _.AppendLine($"Entries:")
        |> _.AppendJoin("\n", cmd.entries |> List.map ArgEntry.getManual)
        |> _.ToString()