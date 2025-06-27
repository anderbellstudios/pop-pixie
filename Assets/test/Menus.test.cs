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

  private IEnumerator Setup() {
    yield return CommonSetup();

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

  [UnityTest, Retry(3)]
  public IEnumerator SelectsButtonsWithKeyboard() {
    yield return Setup();

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

  [UnityTest, Retry(3)]
  public IEnumerator SelectsButtonsOnHover() {
    yield return Setup();

    Button optionsButton = FindButtonByText("Options");
    Button extrasButton = FindButtonByText("Extras");

    yield return Hover(optionsButton);
    AssertSelected(optionsButton);
    Assert.AreEqual(1, MenuSoundCount);

    yield return Hover(extrasButton);
    AssertSelected(extrasButton);
    Assert.AreEqual(2, MenuSoundCount);
  }

  [UnityTest, Retry(3)]
  public IEnumerator DoesNotSelectButtonUntilMouseMove() {
    yield return Setup();
    Button optionsButton = FindButtonByText("Options");
    optionsButton.gameObject.SetActive(false);
    yield return null;
    optionsButton.gameObject.SetActive(true);
    yield return Hover(optionsButton);
    RefuteSelected(optionsButton);
    Assert.AreEqual(0, MenuSoundCount);
    yield return MoveMouseSlightly();
    AssertSelected(optionsButton);
    Assert.AreEqual(1, MenuSoundCount);
  }

  [UnityTest, Retry(3)]
  public IEnumerator ResumesSelectionWhenClosingNestedMenu() {
    yield return Setup();

    Button beginButton = FindButtonByText("Begin");
    Button optionsButton = FindButtonByText("Options");

    yield return Hover(optionsButton);
    Assert.AreEqual(1, MenuSoundCount);

    yield return Click(optionsButton);
    yield return null;
    Assert.AreEqual(2, MenuSoundCount);

    yield return ClickByText("< Back");
    yield return Hover(beginButton);

    Assert.AreEqual(3, MenuSoundCount);
    AssertSelected(optionsButton);
  }

  [UnityTest, Retry(3)]
  public IEnumerator PlaysMenuSoundWhenClosingNestedMenuWithButton() {
    yield return Setup();

    Button optionsButton = FindButtonByText("Options");

    yield return Click(optionsButton);
    yield return null;
    Assert.AreEqual(1, MenuSoundCount);

    yield return PressButton("Cancel");
    Assert.AreEqual(2, MenuSoundCount);
    AssertSelected(optionsButton);
  }
}
#endif
