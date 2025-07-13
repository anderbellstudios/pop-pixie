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
    yield return ClickByText("Options");
    yield return null;

    // Show controller icons stepper
    GameObject.FindObjectOfType<OptionsMenuEvents>().ControllerIconsGameObject.SetActive(true);
  }

  [UnityTest, Retry(3)]
  public IEnumerator PercyScreenshots() {
    yield return Setup();
    yield return TakePercyScreenshot("Options");
    yield return ClickByText("Graphics settings");
    yield return TakePercyScreenshot("Graphics");
    yield return ClickByText("< Back");
    yield return ClickByText("Audio settings");
    yield return TakePercyScreenshot("Audio");
    yield return ClickByText("Set output device");
    yield return TakePercyScreenshot("AudioOutputs");
  }
}
#endif
