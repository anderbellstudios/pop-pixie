using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxController : MonoBehaviour {

  public Text TextBox;
  public float InitialDelay;
  public float WriteDelay;

  [Multiline]
  public string DebugText;

  private string FullText;
  private int WriteProgress;

  public void Write (string text) {
    FullText = text;
    WriteProgress = 0;
    DirectWrite("");

    InvokeRepeating(
      "WriteNextLetter",
      InitialDelay,
      WriteDelay
    );
  }

  void WriteNextLetter () {
    if ( WriteProgress == FullText.Length ) {
      CancelInvoke();
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

	// Use this for initialization
	void Start () {
    Write(DebugText);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
