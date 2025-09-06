#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingRoom4Test : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();

    SceneManager.LoadScene("Training Room 4");
    yield return null;
    yield return AdvanceDialogue();
    GameObject.Find("Lasers").SetActive(false);

    // Make sure time passes normally while moving
    PlayerGameObject.Current.GetComponent<ScriptedMovement>().ScriptedMovementState = false;
  }

  private IEnumerator GoAroundLevel() {
    yield return ScriptedMovement("AfterEnemySpawn");

    KillAllEnemies();

    yield return ScriptedMovement(
      new[] { "TopRight", "BottomRight", "BottomLeft", "BeforeFinish" }
    );
  }

  [UnityTest, Retry(3)]
  public IEnumerator CompletesLevel() {
    yield return Setup();

    yield return TakePercyScreenshot("TrainingRoom4");
    yield return GoAroundLevel();
    yield return AwaitText("00:41", retryInterval: 0.5f, retries: 80);
    MoveUp();
    yield return AwaitText("unsatisfactory", regex: true);
    yield return AdvanceDialogue();
    yield return ClickByText("Try again");
    StopMoving();

    yield return GoAroundLevel();
    MoveUp();
    yield return AwaitText("Congratulations", regex: true);
    yield return AdvanceDialogue();
    yield return ClickByText("Move on");

    yield return AwaitSceneChange("Training Room 5");
  }
}
#endif
