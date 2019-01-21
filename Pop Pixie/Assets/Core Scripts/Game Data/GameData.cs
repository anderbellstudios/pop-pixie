using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;

// An object containing all the data that is 
// persisted from session to session.

[Serializable]
public class GameData {
  public int LevelId;
}
