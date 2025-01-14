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

  private List<Button> OptionButtons = new();
  public override List<Button> LocalInitButtons() => OptionButtons;

  public void SetOptions(List<SelectMenuOption> options, int selectedIndex) {
    ClearOptions();

    for (int i = 0; i < options.Count; i++) {
      CreateOption(options[i], i == selectedIndex);
    }

    // Unity's automatic navigation is terrible
    // TODO: If this works well, use it automatically in AMenu
    for (int i = 0; i < options.Count; i++) {
      Button button = OptionButtons[i];

      button.navigation = new Navigation {
        mode = Navigation.Mode.Explicit,
        selectOnUp = i == 0
          ? null
          : OptionButtons[i - 1],
        selectOnDown = i == options.Count - 1
          ? null
          : OptionButtons[i + 1]
      };
    }

    ProvisionButtons();
  }

  private void ClearOptions() {
    OptionButtons.ForEach(button => Destroy(button.gameObject));
    OptionButtons.Clear();
  }

  private void CreateOption(SelectMenuOption option, bool selected) {
    GameObject optionGameObject = Instantiate(OptionPrefab, OptionsParent);
    optionGameObject.SetActive(true);

    Button button = optionGameObject.GetComponent<Button>();
    OptionButtons.Add(button);

    button.onClick.AddListener(() => {
      Close();
      option.OnSelect();
    });

    if (selected) {
      FirstSelected = button;
    }

    TMP_Text label = optionGameObject.GetComponentInChildren<TMP_Text>();
    label.text = option.Name;
  }
}
