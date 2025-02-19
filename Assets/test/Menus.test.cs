#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenusTest : ABaseTest {
  private int MenuSoundCount = 0;

  [UnitySetUp]
  public IEnumerator Init() {
    SceneManager.LoadScene("Landing");
    yield return new WaitForSeconds(0.5f);

    MenuSoundCount = 0;

    GameObject
      .Find("Menu Sound")
      .GetComponent<PlaySound>()
      .OnPlay.AddListener(() => {
        MenuSoundCount++;
      });
  }

  [UnityTest]
  public IEnumerator SelectsButtonsWithKeyboard() {
    Button beginButton = FindButtonByText("Begin");
    Button optionsButton = FindButtonByText("Options");
    Button extrasButton = FindButtonByText("Extras");
    Button quitButton = FindButtonByText("Quit");

    AssertSelected(beginButton);
    yield return MoveUpAndWait();
    AssertSelected(beginButton);
    Assert.AreEqual(0, MenuSoundCount);
    yield return MoveDownAndWait();
    AssertSelected(optionsButton);
    Assert.AreEqual(1, MenuSoundCount);
    yield return MoveDownAndWait();
    AssertSelected(extrasButton);
    Assert.AreEqual(2, MenuSoundCount);
    yield return MoveDownAndWait();
    AssertSelected(quitButton);
    Assert.AreEqual(3, MenuSoundCount);
    yield return MoveDownAndWait();
    AssertSelected(quitButton);
    Assert.AreEqual(3, MenuSoundCount);
  }

  [UnityTest]
  public IEnumerator SelectsButtonsOnHover() {
    Button optionsButton = FindButtonByText("Options");
    Button extrasButton = FindButtonByText("Extras");

    Hover(optionsButton);
    yield return null;
    AssertSelected(optionsButton);
    Assert.AreEqual(1, MenuSoundCount);

    Hover(extrasButton);
    yield return null;
    AssertSelected(extrasButton);
    Assert.AreEqual(2, MenuSoundCount);
  }

  [UnityTest]
  public IEnumerator DoesNotSelectButtonUntilMouseMove() {
    Button optionsButton = FindButtonByText("Options");
    optionsButton.gameObject.SetActive(false);
    yield return null;
    optionsButton.gameObject.SetActive(true);
    Hover(optionsButton);
    yield return null;
    RefuteSelected(optionsButton);
    Assert.AreEqual(0, MenuSoundCount);
    yield return MoveMouseSlightly();
    AssertSelected(optionsButton);
    Assert.AreEqual(1, MenuSoundCount);
  }

  [UnityTest]
  public IEnumerator ResumesSelectionWhenClosingNestedMenu() {
    Button beginButton = FindButtonByText("Begin");
    Button optionsButton = FindButtonByText("Options");

    Hover(optionsButton);
    yield return null;
    Assert.AreEqual(1, MenuSoundCount);

    Click(optionsButton);
    yield return null;
    Assert.AreEqual(2, MenuSoundCount);

    ClickByText("< Back");
    Hover(beginButton);
    yield return null;

    Assert.AreEqual(3, MenuSoundCount);
    AssertSelected(optionsButton);
  }
}
#endif
