using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectMenuOption {
  public string Name;
  public Action OnSelect;
}

public class SelectMenu : AMenu {
  public GameObject OptionPrefab;
  public Transform OptionsParent;

  protected override bool RegisterButtonsAutomatically() => false;

  public void SetOptions(List<SelectMenuOption> options, int selectedIndex) {
    ClearButtons();

    for (int i = 0; i < options.Count; i++) {
      CreateOption(options[i], i == selectedIndex);
    }
  }

  private void CreateOption(SelectMenuOption option, bool selected) {
    GameObject optionGameObject = Instantiate(OptionPrefab, OptionsParent);
    optionGameObject.SetActive(true);

    Button button = optionGameObject.GetComponent<Button>();

    button.onClick.AddListener(() => {
      Close();
      option.OnSelect();
    });

    if (selected) {
      FirstSelected = button;
    }

    TMP_Text label = optionGameObject.GetComponentInChildren<TMP_Text>();
    label.text = option.Name;

    RegisterButton(button);
  }
}
