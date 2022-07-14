using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DialogueSeenBeforeData {
  public static bool GetSeenBefore(DialoguePage page)
    => ConfigData.Current.Fetch("dialogue-seen-before-" + page.Hash, orSetEqualTo: false);

  public static void SetSeenBefore(DialoguePage page)
    => ConfigData.Current.Set("dialogue-seen-before-" + page.Hash, true);
}
