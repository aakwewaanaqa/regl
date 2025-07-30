namespace Fnm.Pattern.Parse

open Fnm.Helper
open Fnm.Pattern.Parse

type IPatternParseNode =
    interface
        abstract member tryParse : StringCargo -> StringCargo option
    end