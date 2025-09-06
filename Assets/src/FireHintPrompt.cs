using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHintPrompt : MonoBehaviour {
  public GameObject Enemy;
  public float TimeBeforeShow;

  private Stopwatch Stopwatch;

  void Start() {
    Stopwatch = new Stopwatch.PlayingTime();

    InGamePrompt.Current.RegisterSource(
      InGamePrompt.Priority.TutorialFire,
      () =>
        !EnemyUtils.IsDead(Enemy) && Stopwatch.Time() > TimeBeforeShow
          ? "Aim and press [Fire] to destroy your enemy"
          : null
    );
  }
}
