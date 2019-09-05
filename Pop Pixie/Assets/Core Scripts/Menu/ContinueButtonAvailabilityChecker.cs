using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonAvailabilityChecker : MonoBehaviour {

	// Update is called once per frame
	void Update () {
    var button = gameObject.GetComponent<Button>();
    button.interactable = GameData.Exists();
	}
}
