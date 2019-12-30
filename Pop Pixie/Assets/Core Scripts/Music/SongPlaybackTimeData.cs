using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SongPlaybackTimeData {

  public static int Fetch( Song song ) {
    if ( !song.Resume ) return 0;

    return GameData.Fetch(
      KeyString(song), 
      orSetEqualTo: 0
    );
  }

  public static void Record( Song song, int time ) {
    if ( !song.Resume ) return;
    GameData.Set( KeyString(song), time );
  }

  static string KeyString( Song song ) {
    return "song-playback-time-" + song.Name;
  }

}
