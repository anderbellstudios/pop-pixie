#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AssistMenuTest : ABaseTest {
  [UnitySetUp]
  public IEnumerator Init() {
    LoadSceneNotInBuildSettings("Assets/Unity/Scenes/Test Assist Menu.unity");
    yield return AwaitSceneChange("Test Assist Menu");
  }

  [UnityTest]
  public IEnumerator PercyScreenshots() {
    yield return TakePercyScreenshot("Assist");
    HoverByText("Damage reduction");
    AssertSelected(StepperValueByLabel("Damage reduction"));
    yield return null;
    yield return TakePercyScreenshot("AssistOption");
  }
}
#endif
