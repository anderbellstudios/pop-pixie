using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/**
 * Ensures that a button has been up for one frame while the game object was
 * listening for it before detecting a press. This prevents a single button
 * press from activating multiple things in the same frame due to a state
 * change. For example, closing a dialogue box using the space key should not
 * trigger a roll.
 */

public abstract class ButtonPressHelper {
  public abstract void Clear();

  public bool GetButtonPress(string button) {
    bool currentlyDown = WrappedInput.GetButton(button);
    bool isPress = GetButtonAcceptsPress(button) && currentlyDown;
    SetButtonAcceptsPress(button, !currentlyDown);
    return isPress;
  }

  protected abstract bool GetButtonAcceptsPress(string button);
  protected abstract void SetButtonAcceptsPress(string button, bool acceptsPress);
}

public class SingleButtonPressHelper : ButtonPressHelper {
  private bool AcceptsPress = false;

  public override void Clear() {
    AcceptsPress = false;
  }

  protected override bool GetButtonAcceptsPress(string button) => AcceptsPress;

  protected override void SetButtonAcceptsPress(string button, bool acceptsPress) {
    AcceptsPress = acceptsPress;
  }
}

public class MultipleButtonPressHelper : ButtonPressHelper {
  private Dictionary<String, Boolean> ButtonAcceptsPress = new Dictionary<String, Boolean>();

  public override void Clear() {
    foreach (String button in ButtonAcceptsPress.Keys.ToList())
      ButtonAcceptsPress[button] = false;
  }

  protected override bool GetButtonAcceptsPress(string button) => ButtonAcceptsPress.ContainsKey(button)
    ? ButtonAcceptsPress[button]
    : false;

  protected override void SetButtonAcceptsPress(string button, bool acceptsPress) {
    ButtonAcceptsPress[button] = acceptsPress;
  }
}
