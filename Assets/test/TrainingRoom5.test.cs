#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingRoom5Test : ABaseTest {
  [UnitySetUp]
  public IEnumerator Init() {
    SceneManager.LoadScene("Training Room 5");
    yield return null;
    yield return AdvanceDialogue();
    yield return AwaitPlayingState();
  }

  [UnityTest]
  public IEnumerator CompletesLevel() {
    // Too flaky
    // // Wait for camera to settle
    // yield return new WaitForSeconds(1f);
    // yield return TakePercyScreenshot("TrainingRoom5");

    KillAllEnemies();
    yield return AwaitSceneChange("Training Game Results Screen");
  }
}
#endif
