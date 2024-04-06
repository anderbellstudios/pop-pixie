using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using TMPro;

public class Level1Test : ABaseTest {
  [UnityTest]
  public IEnumerator Level1() {
    SandboxGameData();

    SceneManager.LoadScene("Level 1");
    yield return AwaitSceneChange("Level 1");

    yield return AwaitPlayingState();

    yield return ScriptedMovement(new[] {
      "Middle",
      "Top",
      "Elevator"
    });

    AssertHasText("Use an.*Access Terminal.*to", regex: true);

    yield return ScriptedMovement(new[] { "Intel", "Terminal" });

    AssertHasText("Find a.*Keycard.*to", regex: true);

    KillAllEnemies();

    yield return new WaitForSeconds(1.5f);

    AssertHasText("The enemy dropped a.*Keycard", regex: true);

    yield return AdvanceDialogue();

    yield return ScriptedMovement(new[] { "Intel", "Elevator" });

    AssertHasText("Use an.*Access Terminal.*to", regex: true);

    yield return ScriptedMovement(new[] { "Intel", "Terminal" });

    AssertHasText("Press.*to use", regex: true);

    yield return PressButton("Inspect");

    yield return AwaitText("Mentoes Tower brochure", retryInterval: 5f);

    yield return PressButton("Cancel");
    yield return AwaitPlayingState();

    AssertHasText("1.*undiscovered.*Piece of Intel", regex: true);

    yield return ScriptedMovement("Intel");

    AssertHasText("Press.*to steal", regex: true);

    yield return PressButton("Inspect");

    AssertHasText("Presence board");

    yield return PressButton("Cancel");

    RefuteHasText("1.*undiscovered.*Piece of Intel", regex: true);

    yield return ScriptedMovement("Elevator");

    AssertHasText("Press.*to use", regex: true);

    yield return PressButton("Inspect");
    yield return PressButton("Confirm");

    yield return AwaitSceneChange("Elevator");
  }
}
