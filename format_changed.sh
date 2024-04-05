#!/bin/bash
dotnet format --include $(git diff HEAD --diff-filter=AM --name-only | grep '\.cs$') --folder $@
