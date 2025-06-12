#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ElevatorMenuTest : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    SceneManager.LoadScene("Elevator");
    yield return new WaitForSeconds(3.5f);
  }

  [UnityTest, Retry(3)]
  public IEnumerator PercyScreenshots() {
    yield return Setup();
    yield return TakePercyScreenshot("Elevator.Dialogue");
    yield return AdvanceDialogue();
    yield return TakePercyScreenshot("Elevator");
    ClickByText("Continue");
    yield return TakePercyScreenshot("Elevator.Panel");
  }
}
#endif
