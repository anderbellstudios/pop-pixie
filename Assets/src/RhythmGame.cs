using System.Collections.Generic;
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

  private LinearWindow<RhythmGameInput> PressableInputsWindow;

  void Start() {
    PressableInputsWindow = new(Song.Inputs, onExitWindow: MissedInput);
    InputTracks.Init(Song, getTime: () => CurrentTime);
  }

  void Update() {
    PressableInputsWindow.Update(input => input.RelativeTimeAbs(CurrentTime) <= MissThreshold);

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
  }

  private void HandleButtonDown(RhythmGameInputType inputType) {
    /**
     * If there are multiple matching inputs, return the earliest one so that
     * inputs are pressed in the correct order.
     */
    RhythmGameInput input = PressableInputs.Find(input => input.Type == inputType);

    if (input == null) {
      MissedInput(inputType);
    } else {
      HitInput(input);
    }
  }

  private void HitInput(RhythmGameInput input) {
    PressableInputsWindow.RemoveEarly(input);
    InputTracks.HitInput(input);
  }

  private void MissedInput(RhythmGameInput input) {
    Debug.Log("Miss (input not pressed)");
  }

  private void MissedInput(RhythmGameInputType inputType) {
    Debug.Log("Miss (no input found)");
  }

  private float CurrentTime => Time.time;

  private List<RhythmGameInput> PressableInputs => PressableInputsWindow.Current;
}
