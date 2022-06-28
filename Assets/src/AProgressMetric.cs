using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProgressMetric : MonoBehaviour {
  public abstract float Total();
  public abstract float Current();
}
