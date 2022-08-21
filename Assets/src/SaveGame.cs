using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame {

  /**
   * Pop Pixie uses two save files: file0 and file1.
   *
   * file0 is used to store any progress that should not be undone if the
   * player dies, such as starting a new level or reaching a mid-level
   * milestone.
   *
   * file1 is used to store autosaves, which can take place at any time.
   *
   * Some typical workflows involving save data are described below:
   *
   * Starting a new level (see SaveHopper.cs): Once all game objects have
   * started, the level state is serialized into GameData and written to both
   * file0 and file1.
   *
   * Autosaving (see SaveHopper.cs): The level state is serialized and written
   * to file1.
   *
   * Choosing 'Continue' from the main menu (see ContinueGameHopper.cs): The
   * contents of file1 are read into GameData and deserialized, which involves
   * loading the saved level and restoring the state of all game objects.
   *
   * Player death (see PlayerHitPointEvents.cs): The contents of file0 are
   * read into GameData (but not deserialized, so nothing changes onscreen)
   * and are copied into file1. This ensures that if the player quits the game
   * and loads the autosave (file1), the player's progress will be reverted to
   * the latest milestone (i.e. you can't cheat death by quitting).
   *
   * Choosing 'Try again' from the game over screen: The contents of file0 are
   * read into GameData and deserialized.
   */

  public static void WriteSave() {
    WriteTrueSave();
    WriteAutoSave();
  }

  public static void WriteTrueSave() {
    Write("file0");
  }

  public static void WriteAutoSave() {
    Write("file1");
  }

  public static void ReadSave() {
    Read("file0");
  }

  public static void ReadAutoSave() {
    Read("file1");
  }

  public static bool Exists() {
    return new DataOperation(GameData.Current, "file1").Exists();
  }

  static void Write(string fileName) {
    new DataOperation(GameData.Current, fileName).Write();
  }

  static void Read(string fileName) {
    new DataOperation(GameData.Current, fileName).Read();
  }

}
