#!/bin/bash

VERSION=v3

# v3: Upgrade FMOD to 2.02.23
# v2: Add FMOD
# v1: Initial version

set -e

if [ $# -eq 0 ]
then
  echo "Usage: $0 <vendor URL template>" >&2
  exit 1
fi

curl $(echo $1 | sed -e "s/VERSION/$VERSION/") -o vendor.zip
yarn vendor:unpack
rm vendor.zip
