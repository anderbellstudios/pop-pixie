#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class Level1Test : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    SceneManager.LoadScene("Level 1");
    yield return null;
    yield return AwaitPlayingState();
    yield return new WaitForSeconds(1f);

    // Prevent enemy from moving for Percy screenshot
    ImmobiliseAllEnemies();
    DisableEnemyColliders();
  }

  [UnityTest, Retry(3)]
  public IEnumerator CompletesLevel() {
    yield return Setup();
    yield return TakePercyScreenshot("Level1.1");

    // Elevator hint
    yield return ScriptedMovement(new[] {
      "Top",
      "Elevator"
    });
    yield return TakePercyScreenshot("Level1.2");
    AssertHasText("Use an.*Access Terminal.*to", regex: true);

    // Access terminal hint
    yield return ScriptedMovement(new[] { "Intel", "Terminal" });
    AssertHasText("Find a.*Keycard.*to", regex: true);

    yield return KillAllEnemiesAndAwaitKeycard();

    // Elevator hint
    yield return ScriptedMovement(new[] { "Intel", "Elevator" });
    AssertHasText("Use an.*Access Terminal.*to", regex: true);

    // Use access terminal
    yield return ScriptedMovement(new[] { "Intel", "Terminal" });
    AssertHasText("Press.*to use", regex: true);
    yield return PressButton("Inspect");
    yield return new WaitForSeconds(1f);
    yield return TakePercyScreenshot("Level1.AccessTerminal");
    yield return AwaitText("Mentoes Tower brochure", retries: 60);
    yield return TakePercyScreenshot("Level1.MentoesBrochure");
    yield return PressButton("Cancel");
    yield return AwaitPlayingState();

    AssertHasText("1.*undiscovered.*Piece of Intel", regex: true);

    // Intel
    yield return ScriptedMovement("Intel");
    yield return new WaitForSeconds(0.5f);
    AssertHasText("Press.*to steal", regex: true);
    yield return PressButton("Inspect");
    AssertHasText("Presence board");
    yield return PressButton("Cancel");
    RefuteHasText("1.*undiscovered.*Piece of Intel", regex: true);

    // Elevator
    yield return ScriptedMovement("Elevator");
    AssertHasText("Press.*to use", regex: true);
    yield return PressButton("Inspect");
    yield return PressButton("Confirm");

    yield return AwaitSceneChange("Elevator");
  }
}
#endif
