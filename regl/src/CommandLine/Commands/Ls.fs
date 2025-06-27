module Regl.CommandLine.Commands.Ls

open System.IO
open Regl.Exts
open Regl.CommandLine.IO.InOut
open Regl.CommandLine.Types.FlagsAndParams
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds

let cmdName = "ls"

let cmdInfo =
    "list all files or directory from current working directory and outputs them by lines as stdout"

let entry =

    let dFlag = BoolFlag ("-d", "directories")
    let fFlag = BoolFlag ("-f", "files")
    let RFlag = BoolFlag ("-R", "recursively")

    let patternFlag =
        StringFlag ("--pattern", "files or directories matching .net pattern")


    let exeLs : ArgBehaviour =
        fun dto ->
            let pattern = dto.flags.firstOrDefault patternFlag ""
            let pwd = Directory.GetCurrentDirectory ()
            let isFile = dto.flags.containsFlag fFlag
            let isDir = dto.flags.containsFlag dFlag
            let option =
                dto.flags.containsFlag RFlag
                <-?? (SearchOption.AllDirectories, SearchOption.TopDirectoryOnly)
            let files = Directory.GetFiles (pwd, pattern, option)
            let dirs = Directory.GetDirectories (pwd, pattern, option)

            let paths =
                if isFile && isDir then files |> Array.append dirs
                elif isDir then dirs
                else files

            paths |> List.ofArray |> (fun lines -> Out.lines <- lines)

    let cmd = CmdEntry (cmdName, cmdInfo)

    powerset [ dFlag :> IFlag ; fFlag :> IFlag ; RFlag :> IFlag ; patternFlag :> IFlag ]
    |> List.map (fun combo ->
        match combo.Length with
        | 0 -> ArgEntry("lists files or directories").addBehaviour(exeLs)
        | _ -> ArgEntry("lists files or directories").addFlags(combo).addBehaviour(exeLs)
    )
    |> List.iter (fun entry -> cmd.addEntry(entry) |> ignore)

    cmd
