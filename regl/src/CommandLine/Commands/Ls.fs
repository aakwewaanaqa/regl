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

    let cmd = CmdEntry (cmdName, cmdInfo)
    let entry = ArgEntry "lists all files or directories"

    let exeLs : ArgBehaviour =
        fun dto ->
            let pattern = dto.flags.firstOrDefault patternFlag ""
            let pwd = Directory.GetCurrentDirectory ()
            let isFile = dto.flags.containsFlag fFlag
            let isDir = dto.flags.containsFlag dFlag
            let files = Directory.GetFiles (pwd, pattern)
            let dirs = Directory.GetDirectories (pwd, pattern)

            let option =
                dto.flags.containsFlag RFlag
                <-?? (SearchOption.AllDirectories, SearchOption.TopDirectoryOnly)

            let paths =
                if isFile && isDir then files |> Array.append dirs
                elif isDir then dirs
                else files

            paths |> List.ofArray |> (fun lines -> Out.lines <- lines)

    let rec loop (cmd : CmdEntry) (combo : IFlag list list) =
        match combo with
        | head :: tail ->
            let newCmd = cmd.addEntry (entry.addFlags(head).addBehaviour (exeLs))
            loop newCmd tail
        | [] -> cmd

    loop cmd (Flags.getAllPossibilities ([ dFlag ; fFlag ; RFlag ; patternFlag ]))
