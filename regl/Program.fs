namespace Regl

open System
open System.IO
open Regl.CommandLine
open Regl.CommandLine.Commands
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types.Arguments

module Program =
    /// Entry point for the application
    /// Returns 0 to indicate successful execution
    [<EntryPoint>]
    let main (argv : string array) =
        try
            let cmdEntries =
                [| Copy.entry
                   LexFix.entry
                   Ls.entry
                   Match.entry
                   RemoveEmpty.entry
                   Split.entry
                   ToFile.entry
                   GenCommand.Implementation.entry |]

            argv
            |> List.ofArray
            |> Args
            |> executeEntries cmdEntries
            |> ignore

            Out.sendToPipe()
            Debug.close()
            
            0
        with ex ->
            Debug.writeErr ex.Message
            Debug.close()
            
            1
