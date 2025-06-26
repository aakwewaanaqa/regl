namespace Regl

open System
open Regl.CommandLine.Commands
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types.Arguments

module Program =
    /// Entry point for the application
    /// Returns 0 to indicate successful execution
    [<EntryPoint>]
    let main (_ : string array) =
        let cmdEntries =
            [| Copy.entry
               LexFix.entry
               Ls.entry
               Match.entry
               RemoveEmpty.entry
               Split.entry
               ToFile.entry
               GenCommand.Implementation.entry |]

        let raw = Environment.CommandLine
        let args = raw |> Args
        let result = args |> executeEntries cmdEntries

        if result.IsOk then
            0
        else
            1
