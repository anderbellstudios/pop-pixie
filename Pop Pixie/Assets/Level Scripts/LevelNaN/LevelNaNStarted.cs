using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNaNStarted : MonoBehaviour, IDialogueSequenceEventHandler {

  public DialogueManager Dialogue;
  public ScreenFade Fader;
  public Transform Player, Amanda;
  public float Threshold;
  public AEnemyAI InitialAI;
  public SongHopper SongHopper;

  private bool FightTriggered = false;

	// Use this for initialization
	void Start () {
    Fader.Fade("from black", 2.0f);
    StateManager.SetState( State.Playing );
  }

  void Update() {
    if ( StateManager.Isnt( State.Playing ) ) return;
    if ( FightTriggered ) return;

    float distance = Vector3.Distance( Player.position, Amanda.position );

    if ( distance < Threshold ) StartFight();
  }

  void StartFight() {
    FightTriggered = true;
    SongHopper.Hop();
    Dialogue.Play("Dialogue/lNaNd1", this);
  }

  public void SequenceFinished () {
    InitialAI.GainControl();
  }
}
