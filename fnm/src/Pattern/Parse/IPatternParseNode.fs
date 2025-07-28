namespace Fnm.Pattern.Parse

open Fnm.Pattern.Parse

type IPatternParseNode =
    interface
        abstract member tryParse : ParseCargo -> ParseCargo option
    end