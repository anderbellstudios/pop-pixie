#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingGameTowerScene : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
  }

  [UnityTest, Retry(3)]
  public IEnumerator PercyScreenshot() {
    yield return Setup();
    SceneManager.LoadScene("Training Game Tower Scene");
    yield return new WaitForSeconds(5f);
    yield return TakePercyScreenshot("TrainingGameTowerScene");
  }
}
#endif
