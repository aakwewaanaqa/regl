namespace Regl.CommandLine.Types.FlagsAndParams

open System
open System.Collections.Generic

type IFlag =
    abstract member name : string
    abstract member usage : string
    abstract member needInput : bool
    abstract member getVal : string -> ArgVal

    inherit IComparable

and FlagValSet () =
    member val map = Map<IFlag, ArgVal list> [] with get, set

    member s.addVal (f : IFlag) (v : ArgVal) =
        s.map <-
            s.map
            |> Map.change f (function
                | Some vals -> Some (v :: vals)
                | None -> Some [ v ])
        s

    member s.containsFlag (f : IFlag) : bool =
        f |> s.map.ContainsKey

    member s.first<'a> (f : IFlag) : 'a =
        s.map[f] |> List.head |> _.value<'a>()

    member s.firstOrDefault (f : IFlag) (def : 'a) : 'a =
        try
            s.map[f] |> List.map _.value<'a>() |> _.Head
        with ex ->
            def

    member s.findOrDefault (f : IFlag) (def : 'a) : 'a list =
        try
            s.map[f] |> List.map _.value<'a>()
        with ex ->
            [ def ]

    member s.Item
        with get f = s.map[f]

    interface IEnumerable<KeyValuePair<IFlag, ArgVal list>> with
        member s.GetEnumerator () : IEnumerator<KeyValuePair<IFlag, ArgVal list>> =
            new Enumerator (s)

        member s.GetEnumerator () : Collections.IEnumerator =
            new Enumerator (s)

and Enumerator (set : FlagValSet) =
    member e.array = set.map |> Map.toArray
    member val index = 0 with get, set

    interface IEnumerator<KeyValuePair<IFlag, ArgVal list>> with
        member e.Current : KeyValuePair<IFlag, ArgVal list> =
            let k, v = e.array[e.index]
            KeyValuePair (k, v)

        member e.Current : obj =
            let k, v = e.array[e.index]
            KeyValuePair (k, v)

        member e.MoveNext () =
            not (e.index + 1 >= e.array.Length)

        member e.Reset () =
            e.index <- 0

        member e.Dispose () : unit =
            ()
