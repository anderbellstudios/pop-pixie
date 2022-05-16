using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

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

  public void Hit(string eventName) {
    if (Debug.isDebugBuild) {
      Debug.Log("Stubbing Not Analytics hit: " + eventName);
    } else {
      StartCoroutine(SendHit(eventName));
    }
  }

  IEnumerator SendHit(string eventName) {
    WWWForm form = new WWWForm();
    form.AddField("hit[app_id]", AppId);
    form.AddField("hit[event]", eventName);

    using (UnityWebRequest request = UnityWebRequest.Post(Server, form)) {
      yield return request.SendWebRequest();

      if (request.result == UnityWebRequest.Result.Success) {
        Debug.Log("Sent Not Analytics hit");
      } else {
        Debug.Log(request.error);
      }
    }
  }
}
