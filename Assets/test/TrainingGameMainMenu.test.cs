#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingGameMainMenu : ABaseTest {
  [UnitySetUp]
  public IEnumerator Init() {
    SceneManager.LoadScene("Training Game Main Menu");
    yield return new WaitForSeconds(4f);
  }

  [UnityTest]
  public IEnumerator PercyScreenshot() {
    // Wait for zoom to settle
    yield return new WaitForSeconds(1f);
    yield return TakePercyScreenshot("TrainingGameMainMenu");
    yield return null;
  }

  [UnityTest]
  public IEnumerator StartsMissionTraining() {
    Click(GameObject.Find("Mission Training game"));
    yield return AwaitSceneChange("Training Game Tower Scene");
    yield return AwaitSceneChange("Training Room 1", retries: 30);
  }

  [UnityTest]
  public IEnumerator Quits() {
    ClickByText("Quit");
    yield return AwaitSceneChange("Landing");
  }
}
#endif
