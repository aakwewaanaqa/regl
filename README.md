# README

## DESIGN CONCEPT

### Api Source Code Generation

This project is aiming to become a helpful tool for dealing with
front-end api generation. For syncing front-end's protocol and
back-end's sometimes requires time to validate and testing.
So this project aims to generate code by text reading,
or a sick idea, writing command inside the comment.

### The Vision and The Sick Idea

The idea is to read the source file that means the back-end's code,
and then to pass it to a bash script for templating,
and finally to output the echoed lines to a generated file.

The steps:
- `cat <SOURCE-FILE> | regl gen`
- Executes Command in Source File
- Matches Environment Variables
- Processes `<TEMPLATE-FILE>.sh`
- Write Generated File

#### Source File Gen Commands

- `copy <LINE-COUNT>` <br> Copies lines of a source file
- `evcm <LINE-COUNT> <REGEX> <FORMAT> <ENVAR-NAME>` <br>
  Reads lines of the source file <br>
  , and matches them with regex <br>
  , and formats the match with `$0` or `$1` any `$<NUMBER>` as groups <br>
  , and sets to a environment variable for later use. <br>