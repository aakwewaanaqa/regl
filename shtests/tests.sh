#!/bin/bash

cd ../regl/bin/Release/net9.0/linux-x64/
PATH=$PATH:$(pwd)

if [[ -e "$(pwd)/regl" ]]
then
    echo "Start testing."
else
    echo "Working directory is not correct."
fi

echo "Hello World!" | regl copy
echo $PATH | regl split : copy