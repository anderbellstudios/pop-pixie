#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingRoom2Test : ABaseTest {
  [UnitySetUp]
  public IEnumerator Init() {
    SceneManager.LoadScene("Training Room 2");
    yield return null;
    yield return AdvanceDialogue();
  }

  [UnityTest]
  public IEnumerator CompletesLevel() {
    MoveRight();
    yield return new WaitForSeconds(0.25f);
    yield return PressButton("Roll");
    yield return new WaitForSeconds(1.25f);
    yield return PressButton("Roll");
    yield return new WaitForSeconds(1f);
    yield return AdvanceDialogue();
    SceneManager.LoadScene("Training Room 3");
  }
}
#endif
