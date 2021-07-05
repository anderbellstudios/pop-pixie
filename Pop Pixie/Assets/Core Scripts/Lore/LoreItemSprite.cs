using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreItemSprite : AInspectable, IPromptButtonEventHandler {

  public LoreItem LoreItem;
  public string PromptText;
  public DialoguePromptManager PromptManager;

  public override void OnInspect() {
    PromptManager.Display(
      PromptText + "\n(Do you read it?)",
      "Read it",
      "Do not",
      this
    );
  }

  public void ButtonPressed (string button) {
    if ( button == "positive" ) {
      LoreItemData.RecordRead(LoreItem);
      StateManager.SetState( State.Lore );

      LoreManager.Current.Open(LoreItem, () => {
        StateManager.SetState( State.Playing );
      });
    }
  }
}
