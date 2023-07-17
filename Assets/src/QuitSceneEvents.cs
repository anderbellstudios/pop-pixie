using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitSceneEvents : MonoBehaviour {
  void Start() {
    EnhancedDataCollection.LogIfEnabled(() => "Quitting via quit scene");

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
  }
}
