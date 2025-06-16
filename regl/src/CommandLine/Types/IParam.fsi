namespace Regl.CommandLine.Types

open System

/// <summary>
/// The required parameter to be provided with a command
/// </summary>
/// <remarks>
/// Be noticed that, parameters are well-ordered
/// </remarks>
type IParam =
    /// The name we call it to be
    abstract member name : string
    /// The usage to provide explanation for the parameter
    abstract member usage : string
    /// The method to parse the parameter
    [<Obsolete>]
    abstract member parse : string -> string
    /// The method to parse the parameter
    abstract member getVal : string -> FlagVal

    inherit IComparable