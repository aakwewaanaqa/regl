module Regl.CommandLine.Commands.GenCommand.Tpl

open Regl.CommandLine.Types

/// <summary>
/// This executes the tpl command
/// </summary>
val exe : CommandParseResult -> unit

/// <summary>
/// This reads the following lines as context to do template writing by input file path.
/// Before processing the template file, context will be matched with evcm - Environment Variable Context Matcher -,
/// and the environment variable will be matched and set.
/// Be noticed that the tpl command will read the template file in the working directory.
/// </summary>
/// <usage>
/// //#!tpl &lt;LINE-COUNT&gt; &lt;TEMPLATE-FILE-PATH&gt;
/// </usage>
/// <remarks>
/// //#! is just the common way to mark
/// a line of regl gen source file command.
/// It can be anything. If it is the first line
/// of a source file
/// </remarks>
val cmd : CommandBody

