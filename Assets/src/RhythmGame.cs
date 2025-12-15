using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RhythmGameInputType {
  Left = 0,
  Down = 1,
  Up = 2,
  Right = 3,
};

[System.Serializable]
public class RhythmGameInput {
  public RhythmGameInputType Type;
  public float Time;

  public float RelativeTime(float currentTime) => Time - currentTime;

  public float RelativeTimeAbs(float currentTime) => Mathf.Abs(Time - currentTime);
}

[System.Serializable]
public class RhythmGameSong {
  public int BeatsPerMinute;
  public List<RhythmGameInput> Inputs;
}

public class RhythmGame : MonoBehaviour {
  public RhythmGameInputTracks InputTracks;
  public RhythmGameSong Song;
  public float MissThreshold;

  private HashSet<RhythmGameInput> MissedInputs = new();

  public float CurrentTime => Time.time;

  void Start() {
    InputTracks.Init(this);
  }

  void Update() {
    if (WrappedInput.GetButtonDown("Rhythm Game Left")) {
      HandleButtonDown(RhythmGameInputType.Left);
    }

    if (WrappedInput.GetButtonDown("Rhythm Game Down")) {
      HandleButtonDown(RhythmGameInputType.Down);
    }

    if (WrappedInput.GetButtonDown("Rhythm Game Up")) {
      HandleButtonDown(RhythmGameInputType.Up);
    }

    if (WrappedInput.GetButtonDown("Rhythm Game Right")) {
      HandleButtonDown(RhythmGameInputType.Right);
    }

    CheckForLateMisses();
  }

  private void HandleButtonDown(RhythmGameInputType inputType) {
    IEnumerable<RhythmGameInput> eligibleInputs = VisibleInputs.Where(input =>
      input.Type == inputType && input.RelativeTimeAbs(CurrentTime) <= MissThreshold
    );

    if (eligibleInputs.Count() == 0) {
      Debug.Log("Miss (no input found)");
      return;
    }

    RhythmGameInput nearestInput = eligibleInputs.Aggregate(
      (a, b) => a.RelativeTimeAbs(CurrentTime) <= b.RelativeTimeAbs(CurrentTime) ? a : b
    );

    InputTracks.HitInput(nearestInput);
  }

  private void CheckForLateMisses() {
    foreach (RhythmGameInput input in VisibleInputs) {
      if (MissedInputs.Contains(input))
        continue;

      if (input.RelativeTime(CurrentTime) < -MissThreshold) {
        Debug.Log("Miss (input not pressed)");
        MissedInputs.Add(input);
      }
    }
  }

  private IEnumerable<RhythmGameInput> VisibleInputs =>
    InputTracks.SpawnedInputs.Select(spawnedInput => spawnedInput.Input);
}
