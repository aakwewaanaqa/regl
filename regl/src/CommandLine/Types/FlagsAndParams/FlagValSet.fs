namespace Regl.CommandLine.Types.FlagsAndParams

open System

type IFlag =
    abstract member name: string
    abstract member usage: string
    abstract member needInput : bool
    abstract member getVal : string -> ArgVal

    inherit IComparable

and FlagValSet () =
    member val map = Map<IFlag, ArgVal list> [] with get, set

    member s.addVal (f : IFlag) (v : ArgVal) =
        s.map <-
            s.map
            |> Map.change f (function
                | Some vals -> Some (vals @ [ v ])
                | None -> Some [ v ])

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