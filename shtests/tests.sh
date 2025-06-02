#!/bin/bash

# 定义默认路径
OSX_ARM64_PATH="../regl/bin/Release/net9.0/osx-arm64"
LINUX_X64_PATH="../regl/bin/Release/net9.0/linux-x64"

if [[ "${OSTYPE}" == darwin* && "$(arch)" == arm64* ]]; then
  if [ -d "${OSX_ARM64_PATH}" ]; then
    cd "${OSX_ARM64_PATH}" || exit 1
  else
    echo "Error: Directory ${OSX_ARM64_PATH} not found."
    exit 1
  fi
elif [[ "${OSTYPE}" == linux-gnu* && "$(arch)" == x86_64* ]]; then
  if [ -d "${LINUX_X64_PATH}" ]; then
    cd "${LINUX_X64_PATH}" || exit 1
  else
    echo "Error: Directory ${LINUX_X64_PATH} not found."
    exit 1
  fi
else
  echo "Unsupported platform: ${OSTYPE} $(arch)"
  exit 2
fi

export PATH=$PATH:$(pwd)

if [[ -e "$(pwd)/regl" ]]
then
    echo "Start testing."
else
    echo "Working directory is not correct."
    exit 1
fi

echo "Hello World!" | regl copy
echo $(ip a) | regl match wlo1