module Regl.CommandLine.Commands.GenCommand.Shared

open Regl.CommandLine.Commands.GenCommand.Types

val isCmd : string -> bool

val isNotCmd : string -> bool

/// <summary>
/// Stores the command identifier for identifying
/// which line is regl gen source file command.
/// It will be `automatically set` as `the first line`
/// of the source file from piped input,
/// also the first line's trailing spaced will be ignored,
/// but if you gave the first line leading spaces; it will be
/// included in the command identifier as a whole, so be careful.
/// </summary>
/// <remarks>
/// Command line can have leading spaces then the identifier.
/// </remarks>
/// <example>
/// <code>
/// //#!
/// public class SomeClass {
///     //#!copy 1
///     void Main() {
///     }
/// }
/// </code>
/// </example>
val mutable identifier : string

/// <summary>
/// Stores added evcm for tpl command
/// </summary>
/// <remarks>
/// Evcm means 'Environment Variable Context Matcher'
/// </remarks>
val mutable evcms : EnvironmentVariableContextMatcher list