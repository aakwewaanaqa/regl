namespace Fnm.Pattern.Parse.Nodes

open Fnm.Pattern.Parse

type WildCard =
    class
        interface IPatternParseNode with
            member n.tryParse cargo =
                cargo
                |> ParseCargo.tryTakeChar '*'
    end