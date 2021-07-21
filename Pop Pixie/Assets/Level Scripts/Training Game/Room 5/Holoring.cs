using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holoring : MonoBehaviour {
  public float ExpandSpeed;
  public float Lifetime;

  void Update() {
    if (StateManager.Isnt( State.Playing ))
      return;

    float delta = Time.deltaTime * ExpandSpeed;
    transform.localScale += new Vector3(delta, delta, 0);

    Lifetime -= Time.deltaTime;

    if (Lifetime <= 0)
      Destroy(gameObject);
  }

  public void HandleCollidedWithPlayer() {
    PlayerGameObject.Current.GetComponent<HitPoints>().Damage(1);
  }
}
