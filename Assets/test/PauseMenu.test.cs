#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class PauseMenuTest : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    LoadSceneNotInBuildSettings("Assets/Unity/Scenes/Test Level.unity");
    yield return AwaitSceneChange("Test Level");

    // Unlock all intel
    RegisteredLoreItems registeredLoreItems = AssetDatabase.LoadAssetAtPath<RegisteredLoreItems>(
      "Assets/Unity/Scriptable Objects/Lore Items/Registered Lore Items.asset"
    );
    foreach (LoreItem loreItem in registeredLoreItems.LoreItems) {
      LoreItemData.RecordRead(loreItem);
    }

    yield return PressButton("Pause");
    yield return new WaitForSeconds(0.5f);
  }

  [UnityTest, Retry(3)]
  public IEnumerator DiscoveredItemsDebugModeDisabled() {
    yield return Setup();
    Assert.IsFalse(GameObject.FindObjectOfType<DiscoveredItemsMenuEvents>(true).Debug);
  }

  [UnityTest, Retry(3)]
  public IEnumerator PercyScreenshots() {
    yield return Setup();
    yield return TakePercyScreenshot("Pause");
    yield return ClickByText("Pieces of Intel");
    yield return TakePercyScreenshot("Pause.LoreMenu");
    yield return ClickByText("Mentoes Tower brochure");
    yield return TakePercyScreenshot("Pause.LoreWindow");
    yield return PressButton("Cancel");
    yield return PressButton("Cancel");
    yield return ClickByText("Options");
    yield return TakePercyScreenshot("Pause.Options");
    yield return ClickByText("< Back");
    yield return ClickByText("Assist mode");
    yield return TakePercyScreenshot("Pause.Assist");
    yield return ClickByText("< Back");
    yield return ClickByText("Quit game");
    yield return TakePercyScreenshot("Pause.Quit");
  }
}
#endif
