using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class LandingMenuTest : ABaseTest {
  [UnitySetUp]
  public IEnumerator Init() {
    SceneManager.LoadScene(0);
    yield return new WaitForSeconds(0.5f);
  }

  [UnityTest]
  public IEnumerator OpensMainMenu() {
    ClickByText("Begin");
    yield return AwaitSceneChange("Main Menu");
  }

  [UnityTest]
  public IEnumerator OpensOptions() {
    ClickByText("Options");
    AssertHasText("Graphics settings");
    RefuteHasText("Pop Pixie");
    ClickByText("< Back");
    AssertHasText("Pop Pixie");
    yield return null;
  }

  [UnityTest]
  public IEnumerator OpensMissionTraining() {
    ClickByText("Extras");
    ClickByText("Mission Training");
    yield return AwaitSceneChange("Training Game Main Menu");
  }
}
