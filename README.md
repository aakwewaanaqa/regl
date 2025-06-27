# Installation

## LINUX

```sh
cd /opt
sudo curl -LO https://github.com/aakwewaanaqa/regl/releases/download/v0.0.2/regl
sudo curl -LO https://github.com/aakwewaanaqa/regl/releases/download/v0.0.2/regl.pdb
```

# README

## FEATURE

1. LF based statement
2. Micro-command-based
3. F# + Bash
4. Can cope with any language style

### How does it work like?

- Executes command in `source file` by line
- Optionally executes `generating bash file`
- Reads output from commands
- Writes output to `generated file`

### Main command

```sh

# Mainly used
regl gen
```

```sh

# It could be any source file
# This line will output generated text
< source-file.cs regl gen
```

## CODEBASE

### IO

The IO namespace provides core input/output functionality for the code generation system:

- **LinesBuffer**: Manages buffered line operations for source code reading and manipulation
- **InOut**: Handles file input/output operations and command line interactions

These components form the foundation for reading source files, processing commands, and generating output files. The IO
system is designed to efficiently handle both file-based operations and command-line interactions required for the code
generation workflow.

Key responsibilities:

- Source file reading and parsing
- Command execution output buffering
- Generated file writing operations
- Stream-based input/output handling

