#!/bin/bash

if [[ -n $IsFromBody ]]; then
  echo $IsFromBody
fi

if [[ -n $IsFromQuery ]]; then
  echo $IsFromQuery
fi

if [[ -n $Args ]]; then
  echo $Args
fi