using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowsPlayer : MonoBehaviour {

  private Vector3 offset;

	// Use this for initialization
	void Start () {
    offset = transform.position - PlayerGameObject.Current.transform.position;
  }
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = PlayerGameObject.Current.transform.position + offset;
	}
}
