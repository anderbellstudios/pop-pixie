using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class VersionInfo : MonoBehaviour {

  public Text Text;
  public GameObject UpdateButton;

  public string VersionURL;
  public string DownloadURL;

  string CurrentVersion, LatestVersion = "--";

  void Awake() {
    CurrentVersion = Application.version;
    UpdateText();

    InvokeRepeating( "FetchLatestVersion", 0f, 3f );
  }

  void FetchLatestVersion() {
    try {

      using ( WebClient client = new WebClient() ) {
       LatestVersion = client.DownloadString(VersionURL).Trim();
       UpdateText();
       UpdateButton.SetActive(NewVersionAvailable());
       CancelInvoke();
      }

    } catch ( WebException e ) {
      Debug.Log(e);
    }
  }

  void UpdateText() {
    Text.text = String.Format("Current version: {0}\nLatest version: {1}", CurrentVersion, LatestVersion);
  }

  bool NewVersionAvailable() {
    var current = new Version( CurrentVersion );
    var latest = new Version( LatestVersion );

    return latest > current;
  }

  public void ButtonClicked() {
    Application.OpenURL( DownloadURL );
  }

}
