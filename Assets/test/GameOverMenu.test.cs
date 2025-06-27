#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class GameOverTest : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    SceneManager.LoadScene("Game Over");
    yield return new WaitForSeconds(0.5f);
  }

  [UnityTest, Retry(3)]
  public IEnumerator PercyScreenshots() {
    yield return Setup();
    yield return TakePercyScreenshot("GameOver");
    yield return ClickByText("Assist mode");
    yield return TakePercyScreenshot("GameOver.Assist");
  }
}
#endif
