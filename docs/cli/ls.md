# `regl ls` Command
Lists all files or directories from the current working directory and outputs them by lines as stdout.

## Options
| Option | Description |
|--------|-------------|
| `-d` | List directories only |
| `-f` | List files only |
| `-R` | List contents recursively |
| `--pattern <pattern>` | Match files or directories using .NET pattern |

## Examples
1. List all contents in the current directory: `regl ls`
2. List directories only: `regl ls -d`
3. List files only: `regl ls -f`
4. List all contents recursively: `regl ls -dfR`
5. Use pattern matching: `regl ls --pattern '*.txt'`
6. Combined usage: `regl ls -fR '*.txt'`

## Notes
- When both `-d` and `-f` are used, all files and directories will be listed
- Pattern matching uses .NET pattern syntax
- By default (without any options), only files are listed