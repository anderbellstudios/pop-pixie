public class ElevatorData {
  private enum ArrivedFromType { Level, Shop, Load };
  private static ArrivedFromType ArrivedFrom = ArrivedFromType.Level;

  public static bool ArrivedFromLevel => ArrivedFrom == ArrivedFromType.Level;
  public static bool ArrivedFromShop => ArrivedFrom == ArrivedFromType.Shop;
  public static bool ArrivedFromLoad => ArrivedFrom == ArrivedFromType.Load;

  public static void WillArriveFromLevel() {
    ArrivedFrom = ArrivedFromType.Level;
  }

  public static void WillArriveFromShop() {
    ArrivedFrom = ArrivedFromType.Shop;
  }

  public static void WillArriveFromLoad() {
    ArrivedFrom = ArrivedFromType.Load;
  }

  public static int ElevatorRide {
    get {
      return (int)GameData.Current.Fetch("elevator-ride", orSetEqualTo: 0);
    }

    set {
      GameData.Current.Set("elevator-ride", value);
    }
  }
}
