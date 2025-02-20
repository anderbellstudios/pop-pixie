#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

public class TestResolution {
  private static readonly TestResolution Aspect4x3 = new TestResolution(
    width: 1280,
    height: 960,
    aspectWidth: 4,
    aspectHeight: 3
  );

  private static readonly TestResolution Aspect16x9 = new TestResolution(
    width: 1920,
    height: 1080,
    aspectWidth: 16,
    aspectHeight: 9
  );

  private static readonly TestResolution Aspect32x9 = new TestResolution(
    width: 5120,
    height: 1440,
    aspectWidth: 32,
    aspectHeight: 9
  );

  public static readonly TestResolution Default = new TestResolution(
    freeAspect: true
  );

  public static readonly TestResolution TestDefault = Aspect16x9;

  public static readonly TestResolution[] ScreenshotResolutions = {
    Aspect4x3,
    Aspect16x9,
    Aspect32x9
  };

  private int Width, Height, AspectWidth, AspectHeight;
  private bool FreeAspect = false;
  private Action Create;
  private Func<int> GetIndex;
  private Action<int> SetSelectedIndex;

  public TestResolution(
    bool freeAspect = false,
    int width = -1,
    int height = -1,
    int aspectWidth = -1,
    int aspectHeight = -1
  ) {
    FreeAspect = freeAspect;
    Width = width;
    Height = height;
    AspectWidth = aspectWidth;
    AspectHeight = aspectHeight;

    Assembly EditorAssembly = typeof(Editor).Assembly;

    Type GameViewSizes = EditorAssembly.GetType("UnityEditor.GameViewSizes");
    Type GameViewSize = EditorAssembly.GetType("UnityEditor.GameViewSize");
    Type GameViewSizeType = EditorAssembly.GetType("UnityEditor.GameViewSizeType");
    Type GameView = EditorAssembly.GetType("UnityEditor.GameView");

    object gameViewSizes = typeof(ScriptableSingleton<>)
      .MakeGenericType(GameViewSizes)
      .GetProperty("instance")
      .GetValue(null, null);

    MethodInfo getGroup = GameViewSizes.GetMethod("GetGroup");

    object group = getGroup.Invoke(
      gameViewSizes,
      new object[] { (int)GameViewSizeGroupType.Standalone }
    );

    object window = EditorWindow.GetWindow(GameView);

    ConstructorInfo gameViewSizeConstructor = GameViewSize.GetConstructor(
      new Type[] { GameViewSizeType, typeof(int), typeof(int), typeof(string) }
    );

    Create = () => {
      object newGameViewSize = gameViewSizeConstructor.Invoke(
        new object[] { 1, Width, Height, DisplayText }
      );

      getGroup
        .ReturnType
        .GetMethod("AddCustomSize")
        .Invoke(group, new object[] { newGameViewSize });
    };

    GetIndex = () => {
      string[] displayTexts = group
        .GetType()
        .GetMethod("GetDisplayTexts")
        .Invoke(group, null) as string[];

      return Array.IndexOf(displayTexts, DisplayText);
    };

    SetSelectedIndex = (index) => {
      GameView
        .GetProperty(
          "selectedSizeIndex",
          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        )
        .SetValue(window, index, null);
    };
  }

  public string ShortName => String.Format(
    "{0}x{1}",
    AspectWidth,
    AspectHeight
  );

  private string DisplayText => FreeAspect
    ? "Free Aspect"
    : String.Format(
      "Test {0}:{1} ({2}x{3})",
      AspectWidth,
      AspectHeight,
      Width,
      Height
    );

  public void Apply() {
    int index = GetIndex();

    if (index == -1) {
      Create();
      index = GetIndex();
    }

    if (index == -1) {
      throw new Exception("Failed to create resolution");
    }

    SetSelectedIndex(index);
  }
}
#endif
