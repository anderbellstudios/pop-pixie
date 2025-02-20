#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingRoom5Test : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();

    SceneManager.LoadScene("Training Room 5");
    yield return null;

    GameObject
      .Find("Mentoe Hologram")
      .transform
      .Find("AI")
      .gameObject
      .SetActive(false);

    yield return AdvanceDialogue();
    yield return AwaitPlayingState();
  }

  [UnityTest, Retry(3)]
  public IEnumerator CompletesLevel() {
    yield return Setup();
    // Wait for camera to settle
    yield return new WaitForSeconds(1f);
    yield return TakePercyScreenshot("TrainingRoom5");
    KillAllEnemies();
    yield return AwaitSceneChange("Training Game Results Screen");
  }
}
#endif
