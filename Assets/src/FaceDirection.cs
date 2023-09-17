using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirection : MonoBehaviour {

  public MonoBehaviour DirectionManager;
  public Animator Animator;

  void Update() {
    int facing = ((IDirectionManager)DirectionManager).Direction.x > 0 ? 1 : -1;
    Animator.SetInteger("Facing", facing);
  }

}
