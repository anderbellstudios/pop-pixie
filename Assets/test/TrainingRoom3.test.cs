using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TrainingRoom3Test : ABaseTest {
  [UnitySetUp]
  public IEnumerator Init() {
    SceneManager.LoadScene("Training Room 3");
    yield return null;
    yield return AdvanceDialogue();
  }

  [UnityTest]
  public IEnumerator CompletesLevel() {
    yield return ScriptedMovement(new[]{
      "Junction1",
      "Door1Out",
    });

    yield return GoThroughDoor();
    KillAllEnemies(GameObject.Find("HologremsRoom1").transform);

    yield return ScriptedMovement(new[]{
      "Room1Mid",
      "Room1Intel",
    });

    yield return PressButton("Inspect");
    yield return PressButton("Cancel");

    yield return ScriptedMovement(new[]{
      "Room1Mid",
      "Door1In",
      "Door1Out",
      "Junction1",
      "Junction2",
      "Junction2.1",
      "Junction2.2",
      "Door2Out",
    });

    yield return GoThroughDoor();
    KillAllEnemies(GameObject.Find("HologremsRoom2").transform);

    yield return ScriptedMovement(new[]{
      "Room2Mid",
      "Room2Intel",
    });

    yield return PressButton("Inspect");
    yield return PressButton("Cancel");

    yield return ScriptedMovement(new[]{
      "Room2Mid",
      "Door2In",
      "Door2Out",
      "Junction2.2",
      "Junction2.1",
      "Junction2",
      "Junction3",
      "Junction3.1",
      "Junction3.2",
      "Door3Out",
    });

    yield return GoThroughDoor();
    KillAllEnemies(GameObject.Find("HologremsRoom3").transform);

    yield return ScriptedMovement(new[]{
      "Room3Mid",
      "Room3Intel",
    });

    yield return PressButton("Inspect");
    yield return PressButton("Cancel");

    yield return ScriptedMovement(new[]{
      "Room3Mid",
      "Door3In",
      "Door3Out",
      "Junction3.2",
      "Junction3.1",
      "Junction3",
      "Junction4",
      "Junction4.1",
      "Door4Out",
    });

    yield return GoThroughDoor();
    KillAllEnemies(GameObject.Find("HologremsRoom4").transform);

    yield return ScriptedMovement(new[]{
      "Room4Mid",
      "Room4Intel",
    });

    yield return PressButton("Inspect");
    yield return PressButton("Cancel");

    yield return ScriptedMovement(new[]{
      "Room4Mid",
      "Door4In"
    });

    MoveUp();
    yield return AwaitSceneChange("Training Room 4");
  }
}
