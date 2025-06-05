#!/bin/bash

if [[ -n "$must_be_set" ]]; then
#>    public static UniTask<Response<$TResult>> IsUserAlive($TDto dto)
  exit 0
else
  exit 1
fi