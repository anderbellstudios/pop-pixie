using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxController : MonoBehaviour {

  public Text TextBox;
  public Image FaceImage;
  public float InitialDelay;
  public float WriteDelay;
  public IDialoguePageEventHandler EventHandler;
  public GameObject DialogueBox;
  public PromptButtonController PromptButtons;

  private string FullText;
  private int WriteProgress;

  public void Write (string text) {
    FullText = text;
    WriteProgress = 0;
    DirectWrite("");

    CancelInvoke();
    InvokeRepeating(
      "WriteNextLetter",
      InitialDelay,
      WriteDelay
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

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
