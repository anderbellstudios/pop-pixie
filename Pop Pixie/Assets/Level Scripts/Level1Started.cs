using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Started : MonoBehaviour {

  public DialogueManager Dialogue;

	// Use this for initialization
	void Start () {
    StateManager.SetState( State.Dialogue );
    Dialogue.Play("Dialogue/l1d1");
	}
}
