namespace Regl

open System
open System.Text
open Regl.CommandLine
open Regl.CommandLine.Commands
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds

module Program =
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
    
    [<EntryPoint>]
    let main (argv : string array) =
        try

            argv
            |> List.ofArray
            |> List.reduce (fun a b -> $"{a} {b}")
            |> Args
            |> executeEntries cmds
            |> ignore

            Out.sendToPipe()
            Debug.close()
            
            0
        with ex ->
            
            Debug.writeErr ex.Message
            Debug.writeLog manual
            Debug.close()
            
            1
