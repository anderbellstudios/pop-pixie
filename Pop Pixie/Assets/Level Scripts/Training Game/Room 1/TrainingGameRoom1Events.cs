using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingGameRoom1Events : MonoBehaviour {
  public List<GameObject> Targets;

  private bool TargetsDestroyed = false;

  void Start() {
    InGamePrompt.Current.RegisterSource(() =>
      Targets.Any(t => EnemyUtils.IsDead(t))
      ? null
      : "Aim and press <color=#00ffff>[Fire]</color> to shoot the targets"
    );
  }

  void Update() {
    if (!TargetsDestroyed && Targets.All(t => EnemyUtils.IsDead(t))) {
      TargetsDestroyed = true;
      SceneManager.LoadScene("Training Room 2");
    }
  }
}
