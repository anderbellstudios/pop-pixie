using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class LoadAutoSaveMenuItem : MonoBehaviour {

  static string ResumeScene {
		get { return EditorPrefs.GetString("LoadAutoSaveMenuItem.ResumeScene", EditorSceneManager.GetActiveScene().path); }
		set { EditorPrefs.SetString("LoadAutoSaveMenuItem.ResumeScene", value); }
	}

  static bool DoingLoadAutoSave {
		get { return EditorPrefs.GetBool("LoadAutoSaveMenuItem.DoingLoadAutoSave", false); }
		set { EditorPrefs.SetBool("LoadAutoSaveMenuItem.DoingLoadAutoSave", value); }
	}

  static LoadAutoSaveMenuItem() {
    EditorApplication.playModeStateChanged += OnPlayModeStateChange;
  }

  [ MenuItem("Play/Load AutoSave") ]
  public static void LoadAutoSave() {
    DoingLoadAutoSave = true;
    ResumeScene = EditorSceneManager.GetActiveScene().path;
    EditorApplication.NewScene();
    EditorApplication.isPlaying = true;
  }

  static void OnPlayModeStateChange( PlayModeStateChange state ) {
    if ( !DoingLoadAutoSave )
      return;

    switch ( state ) {
      case PlayModeStateChange.EnteredPlayMode:
        GameData.Current.ReadAutoSave();
        GameData.Current.Load();
        break;

      case PlayModeStateChange.EnteredEditMode: 
        DoingLoadAutoSave = false;
        EditorSceneManager.OpenScene(ResumeScene);
        break;
    }
  }

}
