#!/bin/bash

case "$OSTYPE" in
  darwin*) cd ../regl/bin/Release/net9.0/osx-arm64/ || return 1
esac

PATH=$PATH:$(pwd)

if [[ -e "$(pwd)/regl" ]]
then
    echo "Start testing."
else
    echo "Working directory is not correct."
    return 1
fi

echo "Hello World!" | regl copy