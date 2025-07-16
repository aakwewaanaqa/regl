module Regl.CommandLine.Commands.Ls

open System.IO
open System.Text.RegularExpressions
open Microsoft.Extensions.FileSystemGlobbing
open Regl
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
    let ignoreFileFlag = StringFlag("--ignore-file", "for patterns of git ignore")

    let patternFlag =
        StringFlag ("--pattern", "files or directories matching .net pattern")

    let combos = Exts.powerset [
        dFlag :> IFlag
        fFlag :> IFlag
        RFlag :> IFlag
        patternFlag :> IFlag
        ignoreFileFlag :> IFlag
    ]
    
    let exeLs : ArgBehaviour =
        fun dto ->            
            let pattern = dto.flags.firstOrDefault patternFlag "*"
            let isFile = dto.flags.containsFlag fFlag
            let isDir = dto.flags.containsFlag dFlag
            let isRecursively = dto.flags.containsFlag RFlag
            
            let option =
                if isRecursively then SearchOption.AllDirectories
                else SearchOption.TopDirectoryOnly
                
            let ignoreFile = dto.flags.firstOrDefault ignoreFileFlag ""
            
            let paths =           
                let founds =                                            
                    let pwd = Directory.GetCurrentDirectory ()
                    if isFile && isDir then
                        Directory.GetFiles (pwd, pattern, option)
                        |> Array.append (Directory.GetDirectories (pwd, pattern, option))
                    elif isDir then
                        Directory.GetDirectories (pwd, pattern, option)
                    else
                        Directory.GetFiles (pwd, pattern, option)            
                
                if ignoreFile |> System.String.IsNullOrWhiteSpace |> not then
                    let matcher = Matcher()
                    
                    File.ReadAllLines ignoreFile
                    |> Array.map(_.Trim())
                    |> Array.filter(fun pattern -> pattern.StartsWith('#') |> not)
                    |> fun patterns -> matcher.AddExcludePatterns patterns
       
                    matcher.Match(founds).Files
                    |> Seq.map(_.Path)
                    |> List.ofSeq
                else
                    founds
                    |> List.ofArray
                    
            paths |> (fun lines -> Out.lines <- lines)

    CmdEntry(cmdName).addInfo(cmdInfo)
    |> CmdEntry.acceptCombos combos exeLs
