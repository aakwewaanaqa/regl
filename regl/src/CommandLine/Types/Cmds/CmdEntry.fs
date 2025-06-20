namespace Regl.CommandLine.Types.Cmds

open Regl.CommandLine.Types.Arguments

type CmdEntry (name : string, ?info : string) =
    member val entries : ArgEntry list = [] with get, set

    member c.addEntry(entry : ArgEntry) =
        c.entries <- c.entries @ [ entry ]
        c

    override c.ToString() = name

module CmdEntry =
    let validate (args : Args) (cmd : CmdEntry) =
        let rec loop (args : Args) (entries : ArgEntry list) =
            match entries with
            | entry :: tail ->
                match entry |> ArgEntry.validate args with
                | Ok dto ->
                    entry.behaviour dto
                    Ok ()
                | Error _ -> loop args tail
            | [] ->
                let help = entries |> List.map ArgEntry.printHelp |> List.reduce (fun a b -> $"{a}\n{b}")
                Error help

        loop args cmd.entries

