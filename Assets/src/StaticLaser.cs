using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLaser : MonoBehaviour {
  public Transform LaserHumTransform;
  public float MinY, MaxY;

  public void CollidedWithPlayer() {
    PlayerGameObject.Current.GetComponent<HitPoints>().Damage(1);
  }

  void Update() {
    if (!StateManager.Playing)
      return;

    float relativePlayerY = transform.InverseTransformPoint(PlayerGameObject.Current.transform.position).y;
    LaserHumTransform.localPosition = new Vector3(0, Mathf.Clamp(relativePlayerY, MinY, MaxY), 0);
  }
}
