using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowsPlayer : MonoBehaviour {
  private Vector3 offset;

  void Start() {
    offset = transform.position - PlayerGameObject.Position;
  }

  void LateUpdate() {
    transform.position = PlayerGameObject.Current.transform.position + offset;
  }
}
