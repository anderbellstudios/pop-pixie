using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AInterrupt : MonoBehaviour {

  void Start() {
    LocalStart();
  }

  public virtual void LocalStart() { }

  void Update() {
    ALegacyEnemyAI ai = GetComponents<ALegacyEnemyAI>().Where(x => x.InControl).FirstOrDefault();

    if (ai == null)
      return;

    if (!OnlyAIsMatching().IsInstanceOfType(ai))
      return;

    if (ShouldInterrupt(ai))
      ai.RelinquishControlTo(InterruptAI());
  }

  public virtual Type OnlyAIsMatching() {
    return typeof(ALegacyEnemyAI);
  }

  public abstract bool ShouldInterrupt(ALegacyEnemyAI ai);
  public abstract ALegacyEnemyAI InterruptAI();

}
