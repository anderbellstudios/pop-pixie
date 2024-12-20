using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericInspectable : AInspectable {
  public bool Inspectable = true;
  public string PromptText = "Press [Inspect] to ...";
  public UnityEvent OnInspectEvent;

  public void SetInspectable(bool inspectable) {
    Inspectable = inspectable;
  }

  public override bool IsInspectable() => Inspectable;
  public override String AInspectablePromptText() => PromptText;
  public override void OnInspect() => OnInspectEvent.Invoke();
}
