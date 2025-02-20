#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class MainMenuTest : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
  }

  [UnityTest, Retry(3)]
  public IEnumerator PercyScreenshot() {
    yield return Setup();
    SceneManager.LoadScene("Main Menu");
    yield return new WaitForSeconds(0.5f);
    yield return TakePercyScreenshot("MainMenu");
  }

  [UnityTest, Retry(3)]
  public IEnumerator StartsNewGameFirstTime() {
    yield return Setup();
    SceneManager.LoadScene("Main Menu");
    yield return new WaitForSeconds(0.5f);
    ClickByText("New game");
    yield return AwaitSceneChange("Intro Cutscene");
  }

  [UnityTest, Retry(3)]
  public IEnumerator ContinuesGame() {
    yield return Setup();

    ElevatorData.ElevatorRide = 1;
    GameData.Save();
    GameData.Reset();
    Assert.AreEqual(ElevatorData.ElevatorRide, 0);

    SceneManager.LoadScene("Main Menu");
    yield return new WaitForSeconds(0.5f);

    ClickByText("Continue");
    yield return AwaitSceneChange("Elevator");
    Assert.AreEqual(ElevatorData.ElevatorRide, 1);
  }

  [UnityTest, Retry(3)]
  public IEnumerator StartsNewGameSecondTime() {
    yield return Setup();

    ElevatorData.ElevatorRide = 1;
    GameData.Save();

    SceneManager.LoadScene("Main Menu");
    yield return new WaitForSeconds(0.5f);

    ClickByText("New game");
    ClickByText("Cancel");
    ClickByText("New game");
    ClickByText("Reset progress");
    yield return AwaitSceneChange("Intro Cutscene");

    Assert.AreEqual(ElevatorData.ElevatorRide, 0);
  }
}
#endif
