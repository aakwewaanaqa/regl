module regl.Commands.Shared

open System
open regl.Builders.Source

let throwIfNone (a: 'a option) (msg: string) =
    if a.IsNone then raise (Exception(msg)) else ()

let setEnvar key value =
    Environment.SetEnvironmentVariable(key, value)

let getEnvar key = Environment.GetEnvironmentVariable(key)

let hasFlagValue (result: ParseResult option) (name: string) =
    match result with
    | Some r -> r.flags |> Array.exists (fun f -> f.name = name)
    | None -> false

let tryGetFlagValue (result: ParseResult option) (name: string) =
    match result with
    | Some r ->
        r.flags
        |> Array.tryFind (fun f -> f.name = name)
        |> function
            | Some f -> Some (f :?> IInFlag<string>).value
            | None -> None
    | None -> None

let getFlagValue (result: ParseResult option) (name: string) =
    match result with
    | Some r ->
        r.flags
        |> Array.tryFind (fun f -> f.name = name)
        |> function
            | Some f -> (f :?> IInFlag<string>).value
            | None -> raise (Exception $"Flag {name} not found")
    | None -> raise (Exception "No parse result available")

let getParam (result: ParseResult option) (index: int) =
    match result with
    | Some r ->
        if index >= r.parameters.Length then
            raise (Exception $"Parameter index {index} out of range")

        r.parameters[index]
    | None -> raise (Exception "No parse result available")

let writeOut a = Console.Out.Write($"{a}")

let writeOutLine a = Console.Out.WriteLine($"{a}")
