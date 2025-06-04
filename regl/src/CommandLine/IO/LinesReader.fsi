module Regl.CommandLine.IO.LinesReader

val mutable index: int
val mutable lines: string array

/// <summary>
/// Prepares lines from Console.In
/// </summary>
val setFromIn: unit -> unit

/// <summary>
/// Reads line from the current's index position
/// and to advance the index by <see cref="advance"/> if it is true
/// </summary>
val readLines: advance : bool -> count : int -> string

/// <summary>
/// Gets all lines as string from <see cref="lines"/>
/// </summary>
val allLines: unit -> string