#!/bin/bash
# delete this comment
# Unzip vendors into folder
cd Assets/vendor
unzip ../../vendor.zip

# Delete plugins/fmod if exists
rm -f -r ../Plugins/FMOD

# Move vendor/fmod into plugins/fmod
mv FMOD ../Plugins/FMOD

# Remove files if exist
rm -f ../Plugins/FMOD/Resources/FMODStudioSettings.asset{,.meta}