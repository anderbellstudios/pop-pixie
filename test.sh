#!/bin/bash

/Applications/Unity/Hub/Editor/2021.3.32f1/Unity.app/Contents/MacOS/Unity \
  -projectPath . \
  -batchmode \
  -runTests \
  -testPlatform PlayMode \
  -testResults tests.xml

ruby report_test_results.rb
