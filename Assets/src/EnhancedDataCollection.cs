// Uncomment to enable Enhanced Data Collection by default
// #define ENHANCED_DATA_COLLECTION_BUILD

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class EnhancedDataCollection {
  public static readonly bool Enabled =
#if ENHANCED_DATA_COLLECTION_BUILD
    true;
#else
    Environment.GetCommandLineArgs().Contains("-enhancedDataCollection");
#endif

  public static readonly string ClientID = Guid.NewGuid().ToString();

  public static void LogIfEnabled(Func<string> producer) {
    if (Enabled)
      Log(producer());
  }

  private static void Log(string message) {
    Debug.Log(message);

    WWWForm form = new WWWForm();
    form.AddField("client", ClientID);
    form.AddField("message", message);

    UnityWebRequest request = UnityWebRequest.Post("https://log-collector.osbert.me/", form);

    request.SendWebRequest().completed += (AsyncOperation op) => {
      if (request.result != UnityWebRequest.Result.Success) {
        Debug.LogError("Failed to send log message: " + request.error);
      }

      request.Dispose();
    };
  }
}
