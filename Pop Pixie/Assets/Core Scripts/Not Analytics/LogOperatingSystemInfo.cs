using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogOperatingSystemInfo : MonoBehaviour {
  void Start() {
    EnhancedDataCollection.LogIfEnabled(() => "Operating System: " + SystemInfo.operatingSystem);
  }
}
