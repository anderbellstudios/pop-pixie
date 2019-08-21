using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavingTest : MonoBehaviour, ISerializableComponent, ISaveCallbacks {
  public string Name = "Default name";

  [NonSerializedAttribute]
  public string NotMe = "Default NotMe";

  void Update() {
    if ( Input.GetButton("Fire1") && !saving ) {
      StartCoroutine( DoSave() );
    }
  }

  private bool saving = false;

  IEnumerator DoSave() {
    saving = true;

    Debug.Log("Saving");
    GameData.Save();

    yield return new WaitForSeconds(1.0f);

    saving = false;
  }

  public void BeforeSave() {
    Debug.Log("Before Save");
  }
}
