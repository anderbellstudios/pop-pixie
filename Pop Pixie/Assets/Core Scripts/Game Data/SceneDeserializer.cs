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

    foreach ( SerializedGameObject sgo in SerializedScene.GameObjects ) {
      new GameObjectDeserializer( sgo ).Deserialize();
    }
  } 
}
