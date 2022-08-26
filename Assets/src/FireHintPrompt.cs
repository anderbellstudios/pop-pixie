using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHintPrompt : MonoBehaviour {
  public GameObject Enemy;
  public float TimeBeforeShow;

  private float StartTime;

  void Start() {
    InGamePrompt.Current.RegisterSource(99, () =>
      !EnemyUtils.IsDead(Enemy) && PlayingTime.time - StartTime > TimeBeforeShow
      ? "Aim and press [Fire] to destroy your enemy"
      : null
    );

    StartTime = PlayingTime.time;
  }
}
