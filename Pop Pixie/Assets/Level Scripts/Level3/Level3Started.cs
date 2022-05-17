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
  }

  void Update() {
    BossProgressBar.Progress = PhaseScheduler.Progress();
  }

}
