namespace Regl.CommandLine.Commands.GenCommand.Types

open System.Text.RegularExpressions

/// <summary>
/// Environment variable context matcher, used to extract values from context based on regular expressions and set them as environment variables
/// </summary>
type EnvironmentVariableContextMatcher =
    /// <summary>
    /// Initializes a new instance of environment variable context matcher
    /// </summary>
    /// <param name="pattern">Regular expression pattern used to match context</param>
    /// <param name="format">Format string used to extract values from matches, can use replacement groups like $0, $1, etc.</param>
    /// <param name="envarName">Name of the environment variable to set</param>
    new : pattern : Regex * format : string * envarName : string -> EnvironmentVariableContextMatcher

    /// <summary>
    /// Matches the provided context and sets the match results as environment variables
    /// </summary>
    /// <param name="ctx">Context string to match</param>
    /// <returns>No return value</returns>
    /// <remarks>
    /// The match results will be formatted according to the specified format string and set as the specified environment variable.
    /// If the regular expression has multiple matches, each match will cause the environment variable to be set, with the last match becoming the final value.
    /// </remarks>
    member doMatch : ctx : string -> unit
