#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class Level2Test : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    SceneManager.LoadScene("Level 2");
    yield return null;
    yield return AwaitPlayingState(retries: 20);
    yield return new WaitForSeconds(1f);

    // Prevent enemies from moving for Percy screenshot
    ImmobiliseAllEnemies();
    DisableEnemyColliders();
  }

  [UnityTest, Retry(3)]
  public IEnumerator CompletesLevel() {
    yield return Setup();
    yield return TakePercyScreenshot("Level2.1");

    yield return KillAllEnemiesAndAwaitKeycard();

    yield return NavigateTo("Above Left Door");
    yield return AwaitHasText("Press.*to open door", regex: true);
    yield return TakePercyScreenshot("Level2.TopLeft");

    yield return PressButton("Inspect");

    yield return NavigateTo("Left Of Bottom Door");
    yield return AwaitHasText("Press.*to open door", regex: true);
    yield return TakePercyScreenshot("Level2.BottomLeft");

    yield return PressButton("Inspect");

    yield return NavigateTo("Terminal");

    yield return NavigateTo("Below Right Door");
    yield return AwaitHasText("Press.*to open door", regex: true);
    yield return TakePercyScreenshot("Level2.BottomRight");

    yield return PressButton("Inspect");

    yield return NavigateTo("Right Of Top-Right Door");
    yield return AwaitHasText("Press.*to open door", regex: true);
    yield return TakePercyScreenshot("Level2.TopRight");

    yield return PressButton("Inspect");

    yield return NavigateTo("Elevator");
  }
}
#endif
