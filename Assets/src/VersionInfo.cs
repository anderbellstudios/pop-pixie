using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using TMPro;

public class VersionInfo : MonoBehaviour {
  public TMP_Text Text;
  public GameObject UpdateButton;

  public string VersionURL;
  public string DownloadURL;

  string CurrentVersion, LatestVersion = "--";

  void Awake() {
    CurrentVersion = Application.version;
    UpdateText();

    InvokeRepeating("FetchLatestVersion", 0f, 3f);
  }

  void FetchLatestVersion() {
    try {
      using (WebClient client = new WebClient()) {
        LatestVersion = client.DownloadString(VersionURL).Trim();

        UpdateButton.SetActive(
          new Version(CurrentVersion) < new Version(LatestVersion)
        );

        UpdateText();
        CancelInvoke();
      }
    } catch (WebException e) {
      Debug.Log(e);
    }
  }

  void UpdateText() {
    Text.text = String.Format(
      "Current version: {0}\nLatest version: {1}",
      CurrentVersion,
      LatestVersion
    );
  }

  public void ButtonClicked() {
    Application.OpenURL(DownloadURL);
  }
}
