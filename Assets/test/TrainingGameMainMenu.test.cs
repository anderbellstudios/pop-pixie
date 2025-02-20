#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingGameMainMenu : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    SceneManager.LoadScene("Training Game Main Menu");
    yield return new WaitForSeconds(4f);
  }

  [UnityTest, Retry(3)]
  public IEnumerator PercyScreenshot() {
    yield return Setup();
    // Wait for zoom to settle
    yield return new WaitForSeconds(1f);
    yield return TakePercyScreenshot("TrainingGameMainMenu");
    yield return null;
  }

  [UnityTest, Retry(3)]
  public IEnumerator StartsMissionTraining() {
    yield return Setup();
    Click(GameObject.Find("Mission Training game"));
    yield return AwaitSceneChange("Training Game Tower Scene");
    yield return AwaitSceneChange("Training Room 1", retries: 30);
  }

  [UnityTest, Retry(3)]
  public IEnumerator Quits() {
    yield return Setup();
    ClickByText("Quit");
    yield return AwaitSceneChange("Landing");
  }
}
#endif
