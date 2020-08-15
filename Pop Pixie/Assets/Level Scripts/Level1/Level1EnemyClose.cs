using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1EnemyClose : MonoBehaviour, IDialogueSequenceEventHandler {

  public DialogueManager Dialogue;
  public bool Triggered;
  public GameObject Enemy;
  public GameObject Pixie;
  public float ActivationRadius;

	void Update () {
    if ( Triggered || EnemyUtils.IsDead(Enemy) )
      return;

    var dist = Vector3.Distance(
      Enemy.transform.position,
      Pixie.transform.position
    );

    if ( dist < ActivationRadius ) {
      Triggered = true;
      Dialogue.Play("Dialogue/l1d2", this);
    }
	}

  public void SequenceFinished () {
  }
}
