#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class CaptionLineTest : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    LoadSceneNotInBuildSettings("Assets/Unity/Scenes/Test Level.unity");
    yield return AwaitSceneChange("Test Level");

    CaptionLineManager.Current.Play(
      new CaptionLine {
        Text =
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin consequat turpis facilisis erat dignissim lobortis. Etiam fermentum est nec porta venenatis. Cras nec lectus id lacus pretium faucibus. Cras tincidunt posuere tortor in aliquet. Aliquam lobortis imperdiet eros a hendrerit. Mauris ultricies tortor a purus iaculis, nec ultricies purus facilisis.",
        Duration = 2f,
        VoiceLineKey = "",
      }
    );
  }

  [UnityTest, Retry(3)]
  public IEnumerator PercyScreenshot() {
    yield return Setup();
    yield return TakePercyScreenshot("CaptionLine");
  }
}
#endif
