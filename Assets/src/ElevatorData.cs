public class ElevatorData {
  public static bool FromShop = false;

  public static int ElevatorRide {
    get {
      return (int)GameData.Current.Fetch("elevator-ride", orSetEqualTo: 0);
    }

    set {
      GameData.Current.Set("elevator-ride", value);
    }
  }
}
