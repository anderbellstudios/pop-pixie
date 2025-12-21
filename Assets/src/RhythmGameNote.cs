public enum RhythmGameNoteType {
  Left = 0,
  Down = 1,
  Up = 2,
  Right = 3,
};

public class RhythmGameNote {
  public RhythmGameNoteType Type;
  public float Time;
  public float Duration = 0f;

  public bool IsHold => Duration > 0f;
  public bool IsInstant => !IsHold;

  public float RelativeTime(float currentTime, bool end = false) =>
    Time - currentTime + (end ? Duration : 0f);
}
