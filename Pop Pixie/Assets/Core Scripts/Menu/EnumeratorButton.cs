using System;
using System.Collections;
using System.Collections.Generic;

public class EnumeratorButton<T> {

  List<T> Values;
  int Index;
  Action<T> OnChange;

  public EnumeratorButton(List<T> values, T initialValue, Action<T> onChange) {
    Values = values;
    Index = Math.Max(0, values.IndexOf(initialValue));
    OnChange = onChange;
    OnChange.Invoke(Value());
  }

  public T Shift() {
    Index = (Index + 1) % Values.Count;
    T v = Value();
    OnChange.Invoke(v);
    return v;
  }

  public T Value() {
    return Values[Index];
  }

}
