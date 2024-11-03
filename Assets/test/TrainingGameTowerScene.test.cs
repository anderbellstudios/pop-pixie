#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingGameTowerScene : ABaseTest {
  [UnityTest]
  public IEnumerator PercyScreenshot() {
    SceneManager.LoadScene("Training Game Tower Scene");
    yield return new WaitForSeconds(5f);
    yield return TakePercyScreenshot("TrainingGameTowerScene");
  }
}
#endif
