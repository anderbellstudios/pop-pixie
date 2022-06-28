using System;

public class SimulationResultData {
  public static float StartedTime, FinishedTime = 0;
  public static int NumberOfHitsTaken = 0;
  public static int? ObstacleCourseBestTime = null;

  public static float CompletionTime => FinishedTime - StartedTime;
}
