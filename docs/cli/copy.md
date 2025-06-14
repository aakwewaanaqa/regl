---
tags: []
description: 'Copies piped input or redirected input to clipboard.'
---

# Copy

## Notice

1. Needs `apt xsel`

## Usage

```sh
regl copy
```

## Examples

Simply copies a string :
```sh
echo 'Hello World!' | regl copy
```

Copies some serious information :
```sh
echo $PATH | regl copy
```

Copies ip information :
```sh
ip a | regl copy
```

Just copies inet ip :
```sh
ip a | regl match 'inet (192\.168\.[0-9]+\.[0-9]+)' --format '$1' | regl copy
```