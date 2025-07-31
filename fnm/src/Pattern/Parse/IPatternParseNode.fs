namespace Fnm.Pattern.Parse

open Fnm.Helper
open Fnm.Pattern.Parse

type IPatternParseNode<'a> =
    interface
        abstract member tryParse : StringCargo -> ('a * StringCargo) option
    end