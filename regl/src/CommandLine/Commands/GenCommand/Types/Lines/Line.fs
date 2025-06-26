namespace Regl.CommandLine.Commands.GenCommand.Types.Lines

open Regl.CommandLine.Types.Arguments

module Line =
    let mutable cmdBeginning = "//#!"

[<Struct>]
type Line =
    val raw : string
    val isCmd : bool
    val cmdName : string option
    val args : Args option

    new (raw : string)
        =
        if raw.Trim().StartsWith Line.cmdBeginning then
            let raw = raw
            let args = raw[Line.cmdBeginning.Length ..] |> Args
            let cmdName = args[0]
            let args = args.Tail

            { raw = raw
              isCmd = true
              cmdName = Some cmdName
              args = Some args }
        else
            { raw = raw
              isCmd = false
              cmdName = None
              args = None }
