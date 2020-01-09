using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level3Started : MonoBehaviour {

  public ScreenFade Fader;
  public PhaseScheduler PhaseScheduler;
  public HUDBar BossProgressBar;

	void Start () {
    Fader.Fade("from black", 2.0f);
    StateManager.SetState( State.Playing );

    PhaseScheduler.OnPhaseFinished += PhaseFinished;

    GDCall.UnlessLoad( PhaseScheduler.InitPhases );
    GDCall.IfLoad( PhaseScheduler.BeginPhase );
  }

  void PhaseFinished() {
    GameData.Current.Save();
    SaveGame.WriteSave();
  }

  void Update() {
    BossProgressBar.Progress = PhaseScheduler.Progress();
  }

}
