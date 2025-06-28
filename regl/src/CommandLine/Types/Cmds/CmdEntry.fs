namespace Regl.CommandLine.Types.Cmds

open System.Text
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.FlagsAndParams

type CmdEntry (name : string) =
    member c.name = name

    member val info = "" with get, set
    
    member c.addInfo info =
        c.info <- info
        c
    
    member val notice : string = "" with get, set
    /// add notice to a command
    member c.addNotice notice =
        c.notice <- notice
        c    
    
    member val entries : ArgEntry list = [] with get, set
    /// add entry to a command
    /// also sort them by the sum of parameters and flags
    member c.addEntry(entry : ArgEntry) =
        entry.name <- name
        c.entries <-
            c.entries @ [ entry ]
            |> List.sortBy (fun entry -> entry.flags.Length + entry.parameters.Length) 
        c


    
    override c.ToString() = name

module CmdEntry =
    let acceptCombos (combos : IFlag list list) (exe : ArgBehaviour) (cmd : CmdEntry) =
        combos
        |> List.map (fun combo ->
           cmd.addEntry(ArgEntry()
                        |> _.addFlags(combo)
                        |> _.addBehaviour(exe)))
        |> List.last
        
    let collectFlags (cmd : CmdEntry) =
        cmd.entries
        |> List.map _.flags
        |> List.collect id
        |> List.distinct
        |> List.sortBy _.name.Length
        
    let getManual (cmd : CmdEntry) =
        let mutable builder =
            StringBuilder()
                .AppendLine("Command:")
                .AppendLine($"    regl {cmd.name}")
            
        builder <-
            if cmd.info.Length > 0 then
                builder.AppendLine($"        {cmd.info}")
            else
                builder
        
        builder <-
            let flags = cmd |> collectFlags
            if flags.Length > 0 then
                builder.AppendLine("    Flags:")
                       .AppendJoin("\n", flags |> List.map (fun f -> $"        {f.name} {f.info}"))
                       .AppendLine()
            else
                builder
        
        builder <-
            builder.AppendLine("    Entries:")
                   .AppendJoin("\n", cmd.entries |> List.map ArgEntry.getManual)
        
        builder.ToString()