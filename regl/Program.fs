namespace Regl

open System
open Regl.CommandLine.IO
open Regl.CommandLine.Commands
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO.InOut

module Program =
    /// Entry point for the application
    /// Returns 0 to indicate successful execution
    [<EntryPoint>]
    let main (argv : string array) =
        let cmds =
            [ Copy.cmd
              Ls.cmd
              Match.cmd
              RemoveEmpty.cmd
              Split.cmd
              ToFile.cmd
              GenCommand.Implementation.cmd ]

        argv
        |> List.ofArray
        |> tryCommands cmds
        |> function
            | Ok () ->
                Out.sendToPipe ()
                0
            | Error ex ->
                debugLog ex
                1
