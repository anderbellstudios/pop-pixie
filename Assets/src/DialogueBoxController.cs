using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using KoganeUnityLib;

public class DialogueBoxController : MonoBehaviour {
  public TMP_Text Heading, ContinuePrompt;
  public TMP_Typewriter BodyTypewriter;
  public Image FaceImage;

  public bool TypewriterActive = false;
  public UnityEvent OnFinished;

  public void SetHeading(string text) {
    Heading.text = text;
  }

  public void WriteBody(string text, float speed) {
    TypewriterActive = true;

    BodyTypewriter.Play(
      text: text,
      speed: speed,
      onComplete: () => {
        TypewriterActive = false;
        OnFinished.Invoke();
      }
    );
  }

  public void SkipTypewriter() {
    BodyTypewriter.Skip();
  }

  public void SetFace(Sprite face) {
    FaceImage.gameObject.SetActive(face != null);
    FaceImage.sprite = face;
  }

  public void SetContinuePromptVisible(bool visible) {
    ContinuePrompt.color = visible ? Color.white : Color.clear;
  }

  public void Show() {
    gameObject.SetActive(true);
  }

  public void Hide() {
    if (TypewriterActive)
      BodyTypewriter.Skip();

    gameObject.SetActive(false);
  }
}
