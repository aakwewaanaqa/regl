---
tags: []
---

# Ls

Looks up files from the current directory.

## Usage

```
regl ls [-R] [--pattern <search-pattern>]
```

### Options

- -R : Recursively
- --pattern : A search pattern to apply with. (Not `regex`)
You can check up .Net [doc](https://learn.microsoft.com/zh-tw/dotnet/api/system.io.directory.getfiles?view=net-8.0#system-io-directory-getfiles(system-string-system-string))
for the search pattern.

### Outputs

Lines of directories which are files.

## Examples

```sh

regl ls
```