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
    KillAllEnemies();
    yield return AwaitSceneChange("Training Game Results Screen");
  }
}
#endif
