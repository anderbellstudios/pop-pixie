using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void PhaseFinishedEvent();

public class PhaseScheduler : MonoBehaviour, ISerializableComponent {

  public string[] SerializableFields { get; } = { "PhaseId" };

  public List<APhase> Phases;
  public int PhaseId;

  public event PhaseFinishedEvent OnPhaseFinished;

  public void InitPhases() {
    PhaseId = -1;
    NextPhase();
	}

  public void BeginPhase() {
    var phase = Phases[PhaseId];
    phase.Begin( () => PhaseFinished() );
  }

  void NextPhase() {
    PhaseId += 1;
    
    if ( PhaseId < Phases.Count ) {
      BeginPhase();
    }
  }

  void PhaseFinished() {
    NextPhase();
    OnPhaseFinished();
  }

  public float Progress() {
    return TotalBarProgress() / TotalProgressBarAllotment();
  }

  float TotalProgressBarAllotment() {
    return Phases.Sum( phase => phase.ProgressBarAllotment() );
  }

  float TotalBarProgress() {
    return Phases.Sum( phase => phase.ProgressBarValue() );
  }

}
