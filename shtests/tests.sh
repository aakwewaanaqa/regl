#!/bin/bash

cd ../regl/bin/Release/net9.0/linux-x64/ || return 1
PATH=$PATH:$(pwd)

if [[ -e "$(pwd)/regl" ]]
then
    echo "Start testing."
else
    echo "Working directory is not correct."
    return 1
fi

echo "Hello World!" | regl copy
echo "${PATH}" | regl split :

cat regl.deps.json | regl match "System"