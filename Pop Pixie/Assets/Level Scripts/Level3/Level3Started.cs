using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level3Started : MonoBehaviour {
  public HUDBar BossProgressBar;

  public ScreenFade Fader;
  // public AudioClip Music;
  public List<APhase> Phases;
  public int PhaseId;

	// Use this for initialization
	void Start () {
    Fader.Fade("from black", 2.0f);
    PhaseId = -1;
    NextPhase();
	}

  void NextPhase() {
    PhaseId += 1;
    
    if ( PhaseId < Phases.Count ) {
      var phase = Phases[PhaseId];
      phase.Begin( () => NextPhase() );
    }
  }

  void Update() {
    BossProgressBar.Progress = TotalBarProgress() / TotalProgressBarAllotment();
  }

  float TotalProgressBarAllotment() {
    return Phases.Sum( phase => phase.ProgressBarAllotment() );
  }

  float TotalBarProgress() {
    return Phases.Sum( phase => phase.ProgressBarValue() );
  }

}
