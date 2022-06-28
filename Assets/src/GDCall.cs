using System;

public class GDCall {
  static bool FirstTime, Load = false;

  public static void ExpectFirstTime() {
    FirstTime = true;
    Load = false;
  }

  public static void ExpectLoad() {
    FirstTime = false;
    Load = true;
  }

  public static void IfFirstTime( Action callback ) {
    if ( FirstTime )
      callback();
  }

  public static void UnlessFirstTime( Action callback ) {
    if ( !FirstTime )
      callback();
  }

  public static void IfLoad( Action callback ) {
    if ( Load )
      callback();
  }

  public static void UnlessLoad( Action callback ) {
    if ( !Load )
      callback();
  }
}
