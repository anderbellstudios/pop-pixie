using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingRoom1Test : ABaseTest {
  [UnitySetUp]
  public IEnumerator Init() {
    SceneManager.LoadScene("Training Room 1");
    yield return null;
    yield return AdvanceDialogue();
  }

  [UnityTest]
  public IEnumerator CompletesLevel() {
    AssertHasText("Press.*to reload", regex: true);
    yield return PressButton("Reload");
    yield return new WaitForSeconds(1.25f);
    AssertHasText("Aim and press.*to shoot", regex: true);
    KillAllEnemies();
    yield return AwaitSceneChange("Training Room 2");
  }
}
