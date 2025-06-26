namespace Regl.CommandLine.Types.Cmds

open Regl.CommandLine.Types.Arguments

type CmdEntry (name : string, ?info : string) =
    member c.name = name

    member val entries : ArgEntry list = [] with get, set

    member c.addEntry(entry : ArgEntry) =
        c.entries <- c.entries @ [ entry ]
        c

    override c.ToString() = name

