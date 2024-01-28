using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PhaseScheduler))]
public class PhaseSchedulerEditor : Editor {
  public override void OnInspectorGUI() {
    DrawDefaultInspector();

    PhaseScheduler phaseScheduler = (PhaseScheduler)target;

    if (GUILayout.Button("Set phases from children")) {
      List<APhase> phases = new List<APhase>();

      foreach (Transform child in phaseScheduler.transform) {
        APhase phase = child.GetComponent<APhase>();
        if (phase != null) {
          phases.Add(phase);
        }
      }

      phaseScheduler.Phases = phases;
    }
  }
}

