using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class VersionInfo : MonoBehaviour {

  public Text Current, Latest;
  public string VersionURL;

  void Awake() {
    Current.text = Application.version;

    InvokeRepeating( "FetchLatestVersion", 0f, 3f );
  }

  void FetchLatestVersion() {
    try {

      using ( WebClient client = new WebClient() ) {
       Latest.text = client.DownloadString( VersionURL );
       CancelInvoke();
      }

    } catch ( WebException e ) {
      Debug.Log(e);
    }
  }

}
