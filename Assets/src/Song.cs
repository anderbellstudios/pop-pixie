using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Song {
  public AudioClip IntroClip;
  public AudioClip AudioClip;
  public string Name;
  public bool Resume = false;

  public bool HasIntro => IntroClip != null;
  public bool Equals(Song other) => Name == other?.Name;
}
