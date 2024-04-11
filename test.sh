#!/bin/bash

/Applications/Unity/Hub/Editor/2022.3.24f1/Unity.app/Contents/MacOS/Unity \
  -projectPath . \
  -batchmode \
  -runTests \
  -testPlatform PlayMode \
  -testResults tests.xml

ruby report_test_results.rb
