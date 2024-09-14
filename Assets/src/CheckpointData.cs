public static class CheckpointData {
  public static string LastCheckpointId = null;
  public static AData.MemorySave GameDataSave = null;

  public static void SaveGameData() {
    GameDataSave = GameData.Current.ToMemorySave();
  }

  public static void LoadGameData() {
    if (GameDataSave == null) {
      throw new System.Exception("Tried to load null GameDataSave");
    }

    GameData.Current.LoadMemorySave(GameDataSave);
  }

  // Called in ElevatorRide
  public static void Reset() {
    LastCheckpointId = null;
    GameDataSave = null;
  }
}
