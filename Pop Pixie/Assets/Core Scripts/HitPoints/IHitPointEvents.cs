using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitPointEvents {
  void Decreased (HitPoints hp);
  void BecameZero (HitPoints hp);
}
