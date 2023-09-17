using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public delegate void PhaseFinishedEvent();

public class PhaseScheduler : MonoBehaviour, ISerializableComponent {

  public string[] SerializableFields { get; } = { "Running", "PhaseId" };

  public bool BeginFirstPhaseOnStart = true, Running = false;
  public List<APhase> Phases;
  public int PhaseId;

  [SerializeField] public UnityEvent OnPhaseFinished;

  void Start() {
    GDCall.UnlessLoad(() => {
      if (BeginFirstPhaseOnStart)
        BeginFirstPhase();
    });

    GDCall.IfLoad(() => {
      if (Running)
        BeginCurrentPhase();
    });
  }

  public void BeginFirstPhase() {
    Running = true;
    PhaseId = -1;
    NextPhase();
  }

  public void BeginCurrentPhase() {
    var phase = Phases[PhaseId];
    phase.Begin(() => PhaseFinished());
  }

  void NextPhase() {
    PhaseId += 1;

    if (PhaseId < Phases.Count) {
      BeginCurrentPhase();
    } else {
      Running = false;
    }
  }

  void PhaseFinished() {
    NextPhase();
    OnPhaseFinished.Invoke();
  }

  public float Progress() {
    return TotalBarProgress() / TotalProgressBarAllotment();
  }

  float TotalProgressBarAllotment() {
    return Phases.Sum(phase => phase.ProgressBarAllotment());
  }

  float TotalBarProgress() {
    return Phases.Sum(phase => phase.ProgressBarValue());
  }

}
