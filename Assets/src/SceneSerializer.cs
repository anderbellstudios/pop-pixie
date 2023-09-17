using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSerializer : MonoBehaviour {
  public SerializedScene Serialize() {
    return new SerializedScene() {
      Name = SceneName(),
      GameObjects = GameObjects()
    };
  }

  string SceneName() {
    return SceneManager.GetActiveScene().name;
  }

  SerializedGameObject[] GameObjects() {
    return FindObjectsOfType<GameObject>().Select(
      go => new GameObjectSerializer(go).Serialize()
    ).ToArray().Where(sgo => sgo != null).ToArray();
  }
}
