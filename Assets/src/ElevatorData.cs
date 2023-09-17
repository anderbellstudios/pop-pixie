public class ElevatorData {
  public static string NextLevel {
    get {
      return (string)GameData.Current.Fetch("next-level");
    }

    set {
      GameData.Current.Set("next-level", value);
    }
  }
}
