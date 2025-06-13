module Regl.CommandLine.IO.InOut

open System
open System.IO

let mutable In : ReadonlyLinesBuffer = ReadonlyLinesBuffer(ByNone)

let mutable Out : LinesBuffer = LinesBuffer(ByNone)

let debugLog s =
    File.AppendAllText("Debug.log", $"{s}")

let mutable EnvarCache = Map<string, string>(seq {})

let getEnvar k =
    Environment.GetEnvironmentVariable(k)

let setEnvar k v =
    if not (EnvarCache.ContainsKey k) then
        EnvarCache <- EnvarCache.Add(k, getEnvar k)
    Environment.SetEnvironmentVariable(k, v)

let revertEnvars () =
    for kvp in EnvarCache do
        let key = kvp.Key
        let value = kvp.Value
        Environment.SetEnvironmentVariable(key, value)
    EnvarCache <- Map<string, string>(seq {})