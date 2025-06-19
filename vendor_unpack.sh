#!/bin/bash

#unzip vendors into folder
cd Assets/vendor
unzip ../../vendor.zip

#delete plugins/fmod if exists
rm -r ../Plugins/FMOD 2>/dev/null

#move vendor/fmod into plugins/fmod
mv FMOD ../Plugins/FMOD

#remove files if exist
rm -f ../Plugins/FMOD/Resources/FMODStudioSettings.asset{,.meta}





