using System;

public class SimulationResultData {
  public static DateTime? StartedTime, FinishedTime = null;
  public static int NumberOfHitsTaken = 0;
  public static int? ObstacleCourseBestTime = null;

  public static TimeSpan? CompletionTime => (StartedTime.HasValue && FinishedTime.HasValue)
    ? FinishedTime - StartedTime
    : null;
}
