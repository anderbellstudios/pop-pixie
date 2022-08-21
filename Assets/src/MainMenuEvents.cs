using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuEvents : AMenu {
  public TMP_Text PrimaryText, SecondaryText;
  public GameObject SecondaryButton;
  public AMenu ConfirmOverwriteMenu;
  public ContinueGameHopper ContinueGameHopper;
  public NewGameHopper NewGameHopper;

  private Action OnPrimary, OnSecondary;

  public override void LocalStart() {
    if (SaveGame.Exists()) {
      PrimaryText.text = "Continue";
      OnPrimary = () => ContinueGameHopper.Hop();

      SecondaryText.text = "New Game";
      OnSecondary = () => OpenNestedMenu(ConfirmOverwriteMenu);

      SecondaryButton.SetActive(true);
    } else {
      PrimaryText.text = "New Game";
      OnPrimary = () => NewGameHopper.Hop();

      SecondaryButton.SetActive(false);
    }
  }

  public void PrimaryClicked() => OnPrimary.Invoke();
  public void SecondaryClicked() => OnSecondary.Invoke();
}
