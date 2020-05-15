using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;

/* A note on analytics:
 *
 * In the interest of finding a compormise between user privacy and the
 * availability of accurate usage data, Anderbell Studios has developed a
 * custom metrics system that processes only the data we want to collect,
 * and no more. 
 *
 * Pop Pixie collects the version number of the current build at the time
 * of launching the game. This information is collected anonymously and IP
 * addresses are not logged.
 *
 * The source code for the metrics server is available at
 * https://github.com/12joan/not-analytics
 */

public class NotAnalytics : MonoBehaviour {

  public string Server, AppId;

  public void Hit(string path) {
    if ( Debug.isDebugBuild ) {
      Debug.Log("Stubbing Not Analytics hit to " + path);
      return;
    }

    try {

      using ( WebClient client = new WebClient() ) {
        // Fire-and-Forget pattern https://stackoverflow.com/a/2178501
        WebRequest myRequest = WebRequest.Create( Uri(path) );
        ThreadPool.QueueUserWorkItem(o => { myRequest.GetResponse(); });
      }

    } catch ( WebException e ) {
      Debug.Log(e);
    }
  }

  Uri Uri(string path) {
    return new UriBuilder(
      "https",
      Server,
      443,
      AppId + "/" + path
    ).Uri;
  }

}
