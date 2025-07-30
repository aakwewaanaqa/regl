namespace Fnm.Pattern.Parse.Nodes.Bracket

open System
open Fnm.Helper
open Fnm.Pattern.Parse

type CharRange =
    class
        interface IPatternParseNode with
            member n.tryParse cargo =
                if cargo[0] != '-' then
    end