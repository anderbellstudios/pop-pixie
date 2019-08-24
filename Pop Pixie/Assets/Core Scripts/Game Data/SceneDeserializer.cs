using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDeserializer : MonoBehaviour {
  SerializedScene SerializedScene;
  bool WaitingForSceneLoad = false;

  public SceneDeserializer( SerializedScene serializedScene ) {
    SerializedScene = serializedScene;
  }

  public void Deserialize() {
    WaitingForSceneLoad = true;
    SceneManager.sceneLoaded += OnSceneLoaded;
    SceneManager.LoadSceneAsync( SerializedScene.Name );
  }

  public void OnSceneLoaded( Scene scene, LoadSceneMode mode ) {
    if ( !WaitingForSceneLoad )
      return;

    WaitingForSceneLoad = false;

    foreach ( GameObject go in SerializableGameObjects() ) {
      var matchingSerializedGameObjects = SerializedScene.GameObjects.Where(
        sgo => sgo.Guid == go.GetComponent<GuidComponent>().GetGuid()
      ).ToList();

      switch ( matchingSerializedGameObjects.Count ) {
        case 0:
          Destroy(go);
          break;

        case 1:
          var sgo = matchingSerializedGameObjects[0];
          new GameObjectDeserializer( sgo ).Deserialize();
          break;
      }
    } 
  } 

  GameObject[] SerializableGameObjects() {
    return FindObjectsOfType<GameObject>().Where(
      go => go.GetComponent<GuidComponent>() != null
    ).ToArray();
  }
}
