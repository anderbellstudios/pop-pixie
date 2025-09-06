#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingGameOutroScenes : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    SimulationResultData.StartedTime = 0;
    PlayingTime.time = 83;
    SimulationResultData.NumberOfHitsTaken = 16;
    SimulationResultData.ObstacleCourseBestTime = 40;
  }

  [UnityTest, Retry(3)]
  public IEnumerator CompletesScenes() {
    yield return Setup();

    SceneManager.LoadScene("Training Game Results Screen");
    yield return new WaitForSeconds(3f);

    yield return TakePercyScreenshot("TrainingGameResults.Dialogue");

    yield return AdvanceDialogue();
    yield return AdvanceDialogue();

    yield return AwaitText("Press.*to continue", regex: true);

    yield return TakePercyScreenshot("TrainingGameResults.Continue");

    yield return PressButton("Confirm");
    yield return AwaitSceneChange("Training Game Credits");

    yield return new WaitForSeconds(20f);

    yield return AwaitSceneChange("Landing");
  }
}
#endif
