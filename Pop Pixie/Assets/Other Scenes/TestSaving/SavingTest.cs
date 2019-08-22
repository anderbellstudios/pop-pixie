using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingTest : MonoBehaviour, ISerializableComponent, ISaveCallbacks {
  public string[] SerializableFields { get; } = new string[] { "Name" };

  public string Name = "Default name";
  public string NotMe = "Default NotMe";

  void Update() {
    if ( Input.GetButton("Fire1") && !saving ) {
      StartCoroutine( DoSave() );
    }

    if ( Input.GetButton("Roll") && !loading ) {
      StartCoroutine( DoLoad() );
    }
  }

  private bool saving = false;

  IEnumerator DoSave() {
    saving = true;

    Debug.Log("Saving");
    GameData.Save();
    GameData.Write();

    yield return new WaitForSeconds(1.0f);

    saving = false;
  }

  private bool loading = false;

  IEnumerator DoLoad() {
    loading = true;

    Debug.Log("Loading");
    GameData.Read();
    GameData.Load();

    yield return new WaitForSeconds(1.0f);

    loading = false;
  }

  public void BeforeSave() {
    Debug.Log("Before Save");
  }

  public void AfterLoad() {
    Debug.Log("After Load");
  }
}
