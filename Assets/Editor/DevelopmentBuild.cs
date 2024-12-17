/**
 * MIT License
 * 
 * Copyright (c) 2020-present GameCI, Anderbell Studios
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class DevelopmentBuild {
  private const string MetaDataPath = "Assets/src/BuildMetaData.cs";

  public static void Build() {
    ParseCommandLineArguments(out Dictionary<string, string> options);

    BuildTarget buildTarget = (BuildTarget) Enum.Parse(
      typeof(BuildTarget),
      options["buildTarget"]
    );

    List<string> scenes = EditorBuildSettings.scenes
      .Where(scene => scene.enabled)
      .Select(scene => scene.path)
      .ToList();

    // Load a scene that isn't in build settings as the initial scene
    if (options.TryGetValue("initialScene", out string name)) {
      // Spaces cause issues with shell variables, so encode them as underscores
      string nameWithSpaces = name.Replace("_", " ");
      string initialScenePath = $"Assets/Unity/Scenes/{nameWithSpaces}.unity";
      scenes.Insert(0, initialScenePath);
    }

    BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions {
      scenes = scenes.ToArray(),
      locationPathName = options["customBuildPath"],
      target = buildTarget,
      options = UnityEditor.BuildOptions.Development
    };

    BuildMetaData.SetBranchName(options["branchName"]);
    BuildMetaData.SetCommitHash(options["commitHash"]);

    BuildSummary buildSummary;

    try {
      buildSummary = BuildPipeline.BuildPlayer(buildPlayerOptions).summary;
    } finally {
      BuildMetaData.Reset();
    }

    ReportSummary(buildSummary);
    ExitWithResult(buildSummary.result);
  }

  private static void ParseCommandLineArguments(out Dictionary<string, string> providedArguments) {
    providedArguments = new Dictionary<string, string>();
    string[] args = Environment.GetCommandLineArgs();

    // Extract flags with optional values
    for (int current = 0; current < args.Length; current++) {
      int next = current + 1;

      // Ignore arguments not starting with '-'
      bool isFlag = args[current].StartsWith("-");
      if (!isFlag) continue;

      string flag = args[current].TrimStart('-');

      // Parse optional value
      bool flagHasValue = next < args.Length && !args[next].StartsWith("-");
      string value = flagHasValue ? args[next].TrimStart('-') : "";

      Console.WriteLine($"Found flag \"{flag}\" with value \"{value}\".");
      providedArguments.Add(flag, value);
    }
  }

  private static void ReportSummary(BuildSummary summary) {
    Console.WriteLine($"Duration: {summary.totalTime.ToString()}");
    Console.WriteLine($"Warnings: {summary.totalWarnings.ToString()}");
    Console.WriteLine($"Errors: {summary.totalErrors.ToString()}");
    Console.WriteLine($"Size: {summary.totalSize.ToString()} bytes");
  }

  private static void ExitWithResult(BuildResult result) {
    switch (result) {
      case BuildResult.Succeeded:
        Console.WriteLine("Build succeeded!");
        EditorApplication.Exit(0);
        break;
      case BuildResult.Failed:
        Console.WriteLine("Build failed!");
        EditorApplication.Exit(101);
        break;
      case BuildResult.Cancelled:
        Console.WriteLine("Build cancelled!");
        EditorApplication.Exit(102);
        break;
      case BuildResult.Unknown:
      default:
        Console.WriteLine("Build result is unknown!");
        EditorApplication.Exit(103);
        break;
    }
  }
}
