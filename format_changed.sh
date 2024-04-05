#!/bin/bash
./format.sh $(git diff HEAD --diff-filter=AM --name-only | grep '\.cs$') $@
