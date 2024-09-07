#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class MainMenuTest : ABaseTest {
  [UnityTest]
  public IEnumerator StartsNewGameFirstTime() {
    SceneManager.LoadScene("Main Menu");
    yield return new WaitForSeconds(0.5f);
    ClickByText("New game");
    yield return AwaitSceneChange("Intro Cutscene");
  }

  [UnityTest]
  public IEnumerator ContinuesGame() {
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

  [UnityTest]
  public IEnumerator StartsNewGameSecondTime() {
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
