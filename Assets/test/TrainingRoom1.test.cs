#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingRoom1Test : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    SceneManager.LoadScene("Training Room 1");
    yield return null;
  }

  [UnityTest, Retry(3)]
  public IEnumerator CompletesLevel() {
    yield return Setup();
    yield return new WaitForSeconds(3f);
    yield return TakePercyScreenshot("TrainingRoom1.Dialogue");
    yield return AdvanceDialogue();
    yield return TakePercyScreenshot("TrainingRoom1");
    AssertHasText("Press.*to reload", regex: true);
    yield return PressButton("Reload");
    yield return new WaitForSeconds(1.25f);
    AssertHasText("Aim and press.*to shoot", regex: true);
    KillAllEnemies();
    yield return AwaitSceneChange("Training Room 2");
  }
}
#endif
