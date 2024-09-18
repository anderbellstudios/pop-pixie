using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Segment {
  public HUDBar HUDBar;
  public AProgressMetric ProgressMetric;
}

public class SegmentedProgressBar : MonoBehaviour {
  public List<Segment> Segments;

  void Update() {
    Segments.ForEach(segment => {
      segment.HUDBar.SetProgress(segment.ProgressMetric.Current() / segment.ProgressMetric.Total());
    });
  }
}
