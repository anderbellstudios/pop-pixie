#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingRoom2Test : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    SceneManager.LoadScene("Training Room 2");
    yield return null;
    yield return AdvanceDialogue();
  }

  [UnityTest, Retry(3)]
  public IEnumerator CompletesLevel() {
    yield return Setup();
    yield return TakePercyScreenshot("TrainingRoom2");
    MoveRight();
    yield return new WaitForSeconds(0.25f);
    yield return PressButton("Roll");
    yield return new WaitForSeconds(0.85f);
    yield return PressButton("Roll");
    yield return new WaitForSeconds(1f);
    yield return AdvanceDialogue();
    SceneManager.LoadScene("Training Room 3");
  }
}
#endif
