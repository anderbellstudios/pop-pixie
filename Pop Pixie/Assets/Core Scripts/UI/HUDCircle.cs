using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDCircle : MonoBehaviour {

  public Image OuterImage;
  public Image InnerImage;
  public float Progress;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    InnerImage.fillAmount = Progress;
	}
}
