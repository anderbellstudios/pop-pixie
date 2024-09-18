using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level3Started : MonoBehaviour {
  public PhaseScheduler PhaseScheduler;
  public HUDBar BossProgressBar;

  void Update() {
    BossProgressBar.SetProgress(PhaseScheduler.Progress());
  }
}
