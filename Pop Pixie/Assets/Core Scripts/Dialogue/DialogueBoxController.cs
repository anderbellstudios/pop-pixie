using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KoganeUnityLib;

public class DialogueBoxController : MonoBehaviour {
  public TMP_Text Heading;
  public TMP_Typewriter BodyTypewriter;
  public Image FaceImage;

  public bool TypewriterActive = false;

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
      }
    );
  }

  public void SkipTypewriter() {
    BodyTypewriter.Skip();
  }

  public void SetFace(Sprite face) {
    FaceImage.sprite = face;
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
