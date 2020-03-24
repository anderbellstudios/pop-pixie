﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class VersionInfo : MonoBehaviour {

  public Text CurrentText, LatestText;
  public Button UpdateButton;

  public string VersionURL;
  public string DownloadURL;

  string CurrentVersion, LatestVersion;

  void Awake() {
    CurrentVersion = Application.version;

    InvokeRepeating( "FetchLatestVersion", 0f, 3f );
  }

  void FetchLatestVersion() {
    try {

      using ( WebClient client = new WebClient() ) {
       LatestVersion = client.DownloadString( VersionURL );
       CancelInvoke();
      }

    } catch ( WebException e ) {
      Debug.Log(e);
    }
  }

  void Update() {
    CurrentText.text = CurrentVersion;

    if ( LatestVersion != null ) {
      LatestText.text = LatestVersion;

      UpdateButton.interactable = NewVersionAvailable();
    }
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