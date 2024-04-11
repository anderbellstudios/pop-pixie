#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingGameOutroScenes : ABaseTest {
  [UnityTest]
  public IEnumerator CompletesScenes() {
    SceneManager.LoadScene("Training Game Results Screen");
    yield return null;

    yield return AdvanceDialogue();
    yield return AdvanceDialogue();

    yield return AwaitText("Press.*to continue", regex: true);

    yield return PressButton("Confirm");
    yield return AwaitSceneChange("Training Game Credits");

    yield return new WaitForSeconds(20f);

    yield return AwaitSceneChange("Landing");
  }
}
#endif
