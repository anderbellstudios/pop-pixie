using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitPointEvents {
  void Updated (HitPoints hp);
  void Decreased (HitPoints hp);
  void BecameZero (HitPoints hp);
}
