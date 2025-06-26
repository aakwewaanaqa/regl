module Regl.CommandLine.Commands.ToFile

open System.IO
open Regl.CommandLine.IO
open Regl.CommandLine.Types
open Regl.CommandLine.Types.Arguments
open Regl.CommandLine.Types.Cmds
open Regl.CommandLine.Types.FlagsAndParams

let cmdName = "to-file"

let cmdInfo = "writes stdin to a designated file"

let entry =
    let filePathParam = Param ("file-path", "file to write to")
    let appendFlag = BoolFlag ("--append", "writing will append to end of the file")

    let exeWriteToFile : ArgBehaviour =
        fun dto ->
            InOut.In <- ReadonlyLinesBuffer (ByStdIn)
            let path = dto.parameters[Param "<file-path>"].ToString ()
            File.WriteAllText (path, InOut.In.all)

    let exeAppendToFile : ArgBehaviour =
        fun dto ->
            InOut.In <- ReadonlyLinesBuffer (ByStdIn)
            let path = dto.parameters[Param "<file-path>"].ToString ()
            File.AppendAllText (path, InOut.In.all)

    CmdEntry (cmdName, cmdInfo)
    |> _.addEntry(
        ArgEntry "writes new or overwrite file"
        |> _.addParameter(filePathParam)
        |> _.addBehaviour(exeWriteToFile)
    )
    |> _.addEntry(
        ArgEntry "appends old file"
        |> _.addParameter(filePathParam)
        |> _.addFlag(appendFlag)
        |> _.addBehaviour(exeAppendToFile)
    )
