using System;
using System.Collections.Generic;
using System.Linq;

/**
 * Utility class to track a moving window of items in a list for which some
 * predicate is true.
 *
 * The list given to this class must not change, and items must enter the window
 * in ascending order of their index. They can exit the window in any order.
 */
public class LinearWindow<T> {
  public List<T> Current { get; } = new();

  private T[] AllItems;
  private Action<T> OnEnterWindow;
  private Action<T> OnExitWindow;
  private int LastEnteredWindow = -1;

  public LinearWindow(
    IEnumerable<T> allItems,
    Action<T> onEnterWindow = null,
    Action<T> onExitWindow = null
  ) {
    AllItems = allItems.ToArray();
    OnEnterWindow = onEnterWindow;
    OnExitWindow = onExitWindow;
  }

  public void Update(Func<T, bool> predicate) {
    Current
      .Where(item => !predicate(item))
      .ToList()
      .ForEach(item => {
        Current.Remove(item);
        OnExitWindow?.Invoke(item);
      });

    while (LastEnteredWindow < AllItems.Length - 1) {
      T item = AllItems[LastEnteredWindow + 1];

      if (predicate(item)) {
        LastEnteredWindow++;
        Current.Add(item);
        OnEnterWindow?.Invoke(item);
      } else {
        break;
      }
    }
  }

  public void RemoveEarly(T item) {
    Current.Remove(item);
  }
}
