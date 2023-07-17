using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WizardModeChangeSceneMenu : AMenu {
  public GameObject ButtonPrefab;
  public Transform ButtonContainer;

  public override List<Button> LocalInitButtons() {
    List<Button> buttons = new List<Button>();

    for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
      string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
      string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

      GameObject buttonGameObject = Instantiate(ButtonPrefab, ButtonContainer);
      buttonGameObject.SetActive(true);
      WizardModeChangeSceneButton buttonController = buttonGameObject.GetComponent<WizardModeChangeSceneButton>();

      buttonController.SetSceneName(sceneName);
      buttonController.OnClick.AddListener(() => {
        SceneEvents.Current.ChangeScene(sceneName);
      });

      buttons.Add(buttonController.Button);
    }

    return buttons;
  }
}
