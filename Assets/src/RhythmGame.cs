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
}

[System.Serializable]
public class RhythmGameSong {
  public int BeatsPerMinute;
  public List<RhythmGameInput> Inputs;
}

public class RhythmGame : MonoBehaviour {
  public RhythmGameInputTracks InputTracks;
  public RhythmGameSong Song;

  public float CurrentTime => Time.time;

  void Start() {
    InputTracks.Init(this);
  }
}
