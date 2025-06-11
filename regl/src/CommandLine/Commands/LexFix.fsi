module Regl.CommandLine.Commands.LexFix

open Regl.CommandLine.Types

/// <summary>
/// Execute the lex-fix command
/// </summary>
val exe : CommandParseResult -> unit

/// <summary>
/// Fixes the input with a lexical rule
/// </summary>
/// <usage>
/// regl copy
/// </usage>
/// <example>
/// <code>
/// echo 'Task&lt;List&lt;Dto&gt;&gt;&gt;&gt;' | regl lex-fix --scope '&lt;&gt;'
/// </code>
/// </example>
val cmd : CommandBody
