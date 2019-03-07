using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuietMusicDuringDialogue : MonoBehaviour {

  // Update is called once per frame
  void Update () {
    switch ( (State)StateManager.State ) {
      case State.Dialogue:
      case State.DialoguePrompt:
      case State.Lore:
      case State.Cutscene:
        MusicController.Current.SetVolume(0.25f);
        break;

      case State.Playing:
      case State.LoadingLevel:
        MusicController.Current.SetVolume(1.0f);
        break;
    }
  }
}
