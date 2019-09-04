using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxController : MonoBehaviour {

  public Text TextBox;
  public Image FaceImage;
  public float NormalInitialDelay;
  public float FastInitialDelay;
  public float NormalWriteDelay;
  public float FastWriteDelay;
  public GameObject DialogueBox;
  public PromptButtonController PromptButtons;

  private string FullText;
  private int WriteProgress;
  private IDialoguePageEventHandler EventHandler;

  public void Write (string text, IDialoguePageEventHandler event_handler) {
    FullText = text;
    WriteProgress = 0;
    DirectWrite("");
    EventHandler = event_handler;

    CancelInvoke();
    Invoke(
      "WriteNextLetter",
      InitialDelay()
    );
  }

  void WriteNextLetter () {
    if ( WriteProgress == FullText.Length ) {
      FinishPage();
      return;
    }

    WriteProgress += 1;

    DirectWrite(
      FullText.Substring(
        0,
        WriteProgress
      )
    );

    Invoke(
      "WriteNextLetter",
      WriteDelay()
    );
  }

  float InitialDelay() {
    if ( WrappedInput.GetButton("Cancel") ) {
      return FastInitialDelay;
    } else {
      return NormalInitialDelay;
    }
  }

  float WriteDelay() {
    if ( WrappedInput.GetButton("Cancel") ) {
      return FastWriteDelay;
    } else {
      return NormalWriteDelay;
    }
  }

  void DirectWrite (string text) {
    TextBox.text = text;
  }

  public void SetFace (Sprite face) {
    FaceImage.sprite = face;
  }

  public void FinishPage () {
    CancelInvoke();
    DirectWrite(FullText);
    EventHandler.PageFinished();
  }

  public void Show () {
    SetEnabled(true);
  }

  public void Hide () {
    SetEnabled(false);
  }

  void SetEnabled (bool state) {
    DialogueBox.SetActive(state);
  }
}
