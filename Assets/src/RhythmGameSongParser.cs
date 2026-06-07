using System.Collections.Generic;
using MidiParser;

public class RhythmGameSongParser {
  /**
   * Each of the four note types is mapped to F4, A4, C5 and E5 respectively
   * (the gaps on a treble stave in C Major). If a note's duration should be
   * taken into consideration, it should be shifted up one degree in the C Major
   * scale (i.e. on a line instead of a gap).
   */
  private readonly Dictionary<int, (RhythmGameNoteType, bool IsHold)> MIDI_PITCH_DATA = new()
  {
    { 65, (RhythmGameNoteType.Left, false) }, // F4
    { 67, (RhythmGameNoteType.Left, true) }, // G4
    { 69, (RhythmGameNoteType.Down, false) }, // A4
    { 71, (RhythmGameNoteType.Down, true) }, // B4
    { 72, (RhythmGameNoteType.Up, false) }, // C5
    { 74, (RhythmGameNoteType.Up, true) }, // D5
    { 76, (RhythmGameNoteType.Right, false) }, // E5
    { 77, (RhythmGameNoteType.Right, true) }, // F5
  };

  private List<RhythmGameNote> Notes = new();
  private int TicksPerBeat;
  private float BeatsPerMinute = 120f;
  private Dictionary<int, MidiEvent> NoteOnEvents = new();

  public static RhythmGameSong ParseMidi(string path) {
    MidiFile midi = new MidiFile(path);

    if (midi.Tracks.Length != 1)
      throw new System.Exception("MIDI file should have exactly one track");

    RhythmGameSongParser parser = new(ticksPerBeat: midi.TicksPerQuarterNote);
    midi.Tracks[0].MidiEvents.ForEach(parser.ParseMidiEvent);
    return parser.GetSong();
  }

  private RhythmGameSongParser(int ticksPerBeat) {
    TicksPerBeat = ticksPerBeat;
  }

  private void ParseMidiEvent(MidiEvent midiEvent) {
    switch (midiEvent.MidiEventType) {
      case MidiEventType.MetaEvent:
        ParseMetaEvent(midiEvent);
        break;

      case MidiEventType.NoteOn:
        ParseNoteOn(midiEvent);
        break;

      case MidiEventType.NoteOff:
        ParseNoteOff(midiEvent);
        break;
    }
  }

  private void ParseMetaEvent(MidiEvent midiEvent) {
    if (midiEvent.MetaEventType == MetaEventType.Tempo) {
      // TODO: Track a list of tempo change events on the song object
      BeatsPerMinute = (int)midiEvent.Arg2;
    }
  }

  private void ParseNoteOn(MidiEvent midiEvent) {
    /**
     * Store the note on event so that we can reference it from the note off
     * event.
     */
    NoteOnEvents[midiEvent.Note] = midiEvent;
  }

  private void ParseNoteOff(MidiEvent midiEvent) {
    int pitch = midiEvent.Note;
    var (type, isHold) = MIDI_PITCH_DATA[pitch];

    float onTime = TicksToSeconds(NoteOnEvents[pitch].Time);
    float offTime = TicksToSeconds(midiEvent.Time);
    float actualDuration = offTime - onTime;
    float effectiveDuration = isHold ? actualDuration : 0f;

    AddNote(type, onTime, effectiveDuration);
  }

  private void AddNote(RhythmGameNoteType type, float time, float duration) {
    Notes.Add(
      new()
      {
        Type = type,
        Time = time,
        Duration = duration,
      }
    );
  }

  private float TicksToSeconds(float ticks) => ticks / TicksPerBeat * 60f / BeatsPerMinute;

  private RhythmGameSong GetSong() => new() { BeatsPerMinute = BeatsPerMinute, Notes = Notes };
}
