using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using TMPro;

public class InitialMenusTest : ABaseTest {
  [UnityTest]
  public IEnumerator LandingAndMainMenu() {
    SandboxGameData();

    SceneManager.LoadScene(0);

    yield return AwaitSceneChange("Landing");

    AssertHasText(GameObject.Find("Title"), "Pop Pixie");
    RefuteHasText("Graphics settings");

    ClickByText("Options");

    AssertHasText("Graphics settings");
    RefuteHasText("Pop Pixie");

    ClickByText("< Back");

    AssertHasText("Pop Pixie");
    RefuteHasText("Graphics settings");

    yield return new WaitForSeconds(0.5f);

    ClickByText("Begin");

    yield return AwaitSceneChange("Main Menu");

    ClickByText("New game");

    yield return AwaitSceneChange("Level 1");
  }
}
