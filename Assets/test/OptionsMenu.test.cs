#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuTest : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    SceneManager.LoadScene("Landing");
    yield return new WaitForSeconds(0.5f);
    GameObject.Find("Background Animation").SetActive(false);
    ClickByText("Options");
  }

  [UnityTest, Retry(3)]
  public IEnumerator PercyScreenshots() {
    yield return Setup();
    yield return TakePercyScreenshot("Options");
    ClickByText("Graphics settings");
    yield return TakePercyScreenshot("Graphics");
    ClickByText("< Back");
    ClickByText("Audio settings");
    yield return TakePercyScreenshot("Audio");
    ClickByText("Set output device");
    yield return TakePercyScreenshot("AudioOutputs");
  }
}
#endif
