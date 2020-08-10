public class ElevatorData {
  public static string NextLevel {
    get {
      return GameData.Current.Fetch("next-level");
    }

    set {
      GameData.Current.Set("next-level", value);
    }
  }
}
