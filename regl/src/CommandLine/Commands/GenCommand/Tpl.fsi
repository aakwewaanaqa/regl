module Regl.CommandLine.Commands.GenCommand.Tpl

open Regl.CommandLine.Types

/// <summary>
/// This reads the following lines as context to do template writing by subprocess's output
/// Before processing the template file, context will be matched with Evcm - Environment Variable Context Matcher -
/// </summary>
/// <remarks>
///     <see cref="AddEvcm"/>
/// </remarks>
val public cmd : CommandBody

/// <summary>
/// This executes the tpl command
/// </summary>
val private exe : result : ParseResult option -> unit