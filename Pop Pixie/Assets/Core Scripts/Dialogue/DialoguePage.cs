using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePage {
  public string FaceId;
  public string Text;

  public Sprite Face () {
    return Resources.Load<Sprite>("Faces/" + FaceId);
  }
}
