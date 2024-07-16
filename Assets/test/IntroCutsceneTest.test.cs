#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class IntroCutsceneTest : ABaseTest {
  [UnityTest]
  public IEnumerator CompletesScene() {
    SceneManager.LoadScene("Intro Cutscene");
    yield return AwaitSceneChange("Level 1", retries: 60);
  }
}
#endif
