# `regl copy` Command
Copies piped input to the system clipboard. This command is designed to work with pipeline input and transfer its contents to your clipboard for easy pasting elsewhere.

## Options
This command does not accept any options. It simply takes the piped input and copies it to the clipboard.

## Examples
1. Copy file contents to clipboard: `cat file.txt | regl copy`
2. Copy directory listing to clipboard: `regl ls | regl copy`
3. Copy command output to clipboard: `regl ls -R | regl copy`

## Notes
- This command requires `xsel` to be installed on Linux platforms
- The command works with any text-based piped input
- The entire input is copied as-is to the system clipboard
- If no input is piped to the command, no action will be taken