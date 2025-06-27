namespace Regl.CommandLine.Commands.GenCommand.Types.Lines

open Regl.CommandLine.Types.Arguments

[<Struct>]
type SourceLine =
    val isCmd : bool
    val args : Args
    val raw : string

    new (cmdBeginning : string, raw : string)
        =
        let trimmed = raw.Trim ()

        if trimmed.StartsWith (cmdBeginning) then
            let cmdLine = trimmed.Substring (cmdBeginning.Length)
            let args = cmdLine |> Args
            let raw = raw

            { isCmd = true
              args = args
              raw = raw }
        else
            { isCmd = false
              args = Args ""
              raw = raw }
