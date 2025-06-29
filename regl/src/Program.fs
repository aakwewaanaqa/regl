module Regl.Program

open System
open Regl.CommandLine
open Regl.CommandLine.Commands.Shared
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds
open Regl.Exceptions

[<EntryPoint>]
let main (argv : string array) =
    let returnCode =
        try

            match argv.Length > 0 with
            | false -> raise(CLIEmptyArgException())
            | true -> ()
            
            argv
            |> List.ofArray
            |> List.reduce (fun a b -> $"{a} {b}")
            |> Args
            |> executeEntries Shared.cmds
            |> ignore

            Out.sendToPipe()
            Debug.close()
            
            0
        with
        | :? CLIEmptyArgException
        | :? CLICommandNotFoundException as ex ->
            Debug.writeErr ex
            Debug.writeLog "Recommending commands ~"
            Debug.writeLog Shared.manual
            1
        | :? CLIEntryNotValidException as ex ->
            Debug.writeErr ex
            Debug.writeLog $"Recommending {ex.cmdName} usages ~"
            Debug.writeLog("\n" +
                (Shared.cmds
                |> Array.find (fun cmd -> cmd.name = ex.cmdName)
                |> CmdEntry.getManual))
            1
        | ex ->
            Debug.writeErr ex
            1
    
    Debug.close()
    returnCode