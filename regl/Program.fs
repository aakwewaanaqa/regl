namespace Regl

open Regl.CommandLine.IO
open Regl.CommandLine.Commands
open Regl.CommandLine.Commands.Shared

module Program =
    /// Entry point for the application
    /// Returns 0 to indicate successful execution
    [<EntryPoint>]
    let main argv =

        InOut.In <- LinesBuffer(ByConsoleIn)

        [|
            Copy.cmd
            Ls.cmd
            Match.cmd
            RemoveEmpty.cmd
            Split.cmd
            ToFile.cmd
            GenCommand.Implementation.cmd
        |]
        |> tryCommands argv
        |> function
            | 0 ->
                InOut.Out.sendToPipe ()
                0
            | n -> n
