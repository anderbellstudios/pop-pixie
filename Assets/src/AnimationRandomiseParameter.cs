using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRandomiseParameter : MonoBehaviour {

  public bool RandomiseOnAwake = true;
  public Animator Animator;
  public string ParameterName;
  public float MinValue, MaxValue;

  void Awake() {
    if (RandomiseOnAwake) 
      Perform();
  }

  public void Perform() {
    Animator.SetFloat(ParameterName, Random.Range(MinValue, MaxValue));
  }

}
