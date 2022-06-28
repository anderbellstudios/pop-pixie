using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitPointsProgressMetric : AProgressMetric {
  public List<HitPoints> HitPointses;

  public override float Total()
    => HitPointses.Select(hp => hp.Maximum).Sum();

  public override float Current()
    => HitPointses.Select(hp => hp.Current).Sum();
}
