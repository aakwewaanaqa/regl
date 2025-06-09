module Regl.CommandLine.Commands.GenCommand.Implementation

open Regl.CommandLine.Types

/// <summary>
/// All the commands that belongs to gen command
/// which can be used in the source file
/// </summary>
val subCmds : CommandBody list

/// <summary>
/// Execution of the gen command
/// </summary>
val exe : CommandParseResult -> unit

/// <summary>
/// The gen command
/// </summary>
/// <usage>
/// regl gen [--file &lt;FILE-PATH&gt;]
/// </usage>
val cmd : CommandBody