#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class WeaponSwitcherTest : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();

    LoadSceneNotInBuildSettings("Assets/Unity/Scenes/Test Level.unity");
    yield return AwaitSceneChange("Test Level");

    // Unlock all weapons
    foreach (Weapon weapon in PlayerWeapons.Current.AllWeapons) {
      if (!weapon.StartingWeapon) {
        BoughtWeaponsData.SetBought(weapon.Id, true);
      }
    }

    // Reload the scene so that the change takes effect
    LoadSceneNotInBuildSettings("Assets/Unity/Scenes/Test Level.unity");
    yield return AwaitSceneChange("Test Level");

    // Ensure direction is consistent
    SetViewportMousePosition(1f, 0.5f);

    yield return ButtonDown("Change Weapon");
    yield return new WaitForSeconds(0.5f);
  }

  [UnityTest, Retry(3)]
  public IEnumerator PercyScreenshot() {
    yield return Setup();
    yield return TakePercyScreenshot("WeaponSwitcher");
  }
}
#endif
