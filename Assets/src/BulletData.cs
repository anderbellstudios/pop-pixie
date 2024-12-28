using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour {
  public float Damage;
  public float Lifetime;
  public GameObject Originator;
  public float CounterAttackSpeed;
  public float CounterAttackDamage;
  public Func<Vector3> GetDirection;
  public Collider2D IgnoreCollider;
}
