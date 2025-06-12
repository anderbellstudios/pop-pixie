using UnityEngine;

public class LevelCompletionData {
  public static void CompleteLevel(int level) {
    if (level <= LevelCompleted) {
      Debug.LogError("Cannot set LevelCompleted to lower value");
      return;
    }

    LevelCompleted = level;
    PlayElevatorRide = true;
  }

  public static void PlayedElevatorRide() {
    PlayElevatorRide = false;
  }

  public static int LevelCompleted {
    get {
      return (int)GameData.Current.Fetch("level-completed", orSetEqualTo: 0);
    }

    private set {
      GameData.Current.Set("level-completed", value);
    }
  }

  public static bool PlayElevatorRide {
    get {
      return (bool)GameData.Current.Fetch("play-elevator-ride", orSetEqualTo: true);
    }

    private set {
      GameData.Current.Set("play-elevator-ride", value);
    }
  }
}
