namespace Fnm.Pattern.Parse.Nodes

open Fnm.Pattern.Parse

type Negate =
    class
        interface IPatternParseNode with
            override n.tryParse cargo =
                cargo
                |> ParseCargo.tryTakeChar '!'
    end