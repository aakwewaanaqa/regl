# README

## How to use
You can see examples in ./shtests

### Copying txt
```sh
echo "Hello World!" | regl copy
```

### Splitting to lines
```sh
echo $PATH | regl split :
```

> Commands can be queued, like:
> ```sh
> echo $PATH | regl split : copy
> ```
> It will split by : into lines then copy the whole lines.

### 