using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePhase : APhase {

  public DialogueManager DialogueManager;
  public string DialoguePath;

	public override void LocalBegin () {
    // DialogueManager.Play(DialoguePath, this);
    PhaseFinished();
  }

}
