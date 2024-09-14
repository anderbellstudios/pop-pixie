using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

[ExecuteInEditMode]
public class PersistentId : MonoBehaviour {
  public string Id = null;

#if UNITY_EDITOR
  void Update() {
    if (Application.isPlaying) return;
    string id = GlobalObjectId.GetGlobalObjectIdSlow(gameObject).ToString();
    if (id != Id) {
      Id = id;
      PrefabUtility.RecordPrefabInstancePropertyModifications(this);
      EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
    }
  }
#endif
}
