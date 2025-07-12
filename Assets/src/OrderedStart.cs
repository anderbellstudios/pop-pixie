using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderedStart : MonoBehaviour {
  private class OrderedStartEntry {
    public System.Action Callback;
    public float Order;
  }

  private static List<OrderedStartEntry> SortedOrderedStartEntries = new List<OrderedStartEntry>();

  // Must only be called from the Awake method
  public static void Add(System.Action callback, float order) {
    OrderedStartEntry entry = new OrderedStartEntry() { Callback = callback, Order = order };

    /**
     * Insertion sort.
     *
     * Example 1:
     *   Suppose we have [] and want to insert 5
     *   firstGreaterThanIndex is -1
     *   insertIndex is 0
     *   Resulting list is [5]
     *
     * Example 2:
     *   Suppose we have [1, 2, 3] and want to insert 5
     *   firstGreaterThanIndex is -1
     *   insertIndex is 3
     *   Resulting list is [1, 2, 3, 5]
     *
     * Example 3:
     *   Suppose we have [6, 7, 8] and want to insert 5
     *   firstGreaterThanIndex is 0
     *   insertIndex is 0
     *   Resulting list is [5, 6, 7, 8]
     *
     * Example 4:
     *   Suppose we have [4, 5, 6] and want to insert 5
     *   firstGreaterThanIndex is 1
     *   insertIndex is 1
     *   Resulting list is [4, 5 (new), 5 (old), 6]
     */
    int firstGreaterThanIndex = SortedOrderedStartEntries.FindIndex(e => e.Order >= order);

    int insertIndex =
      firstGreaterThanIndex == -1 ? SortedOrderedStartEntries.Count : firstGreaterThanIndex;

    SortedOrderedStartEntries.Insert(insertIndex, entry);
  }

  void Start() {
    SortedOrderedStartEntries.ForEach(entry => entry.Callback());
  }

  void OnDestroy() {
    SortedOrderedStartEntries.Clear();
  }

  // Helpers

  public static float After(params float[] orders) => Mathf.Max(orders) + 1;

  public static float Before(params float[] orders) => Mathf.Min(orders) - 1;

  public static float Between(float[] afterOrders, float[] beforeOrders) {
    float maxAfter = Mathf.Max(afterOrders);
    float minBefore = Mathf.Min(beforeOrders);

    if (minBefore <= maxAfter) {
      throw new System.Exception("OrderedStart.Between: minBefore <= maxAfter");
    }

    return maxAfter + ((minBefore - maxAfter) / 2f);
  }

  public static float Between(float afterOrder, float beforeOrder) =>
    Between(new float[] { afterOrder }, new float[] { beforeOrder });
}
