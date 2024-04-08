using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PathfindingTest : ABaseTest {
  [UnityTest]
  public IEnumerator CompletesLevel() {
    for (int i = 0; i < 5; i++) {
      LoadSceneNotInBuildSettings("Assets/Unity/Scenes/Test Pathfinding.unity");
      yield return AwaitSceneChange("Test Pathfinding");

      GameObject agents = GameObject.Find("Agents");
      TestPathfindingBadZone badZone = GameObject.Find("BadZone").GetComponent<TestPathfindingBadZone>();

      foreach (Transform agent in agents.transform) {
        agent.gameObject.GetComponent<NavigateToPoint>().enabled = true;
      }

      yield return AwaitCondition(
        condition: () => agents.transform.childCount == 0,
        retries: 30,
        message: "All agents should reach their destination"
      );

      Assert.False(badZone.Failed, "Agent should not touch bad zone");

      SceneManager.LoadScene("Landing");
      yield return null;
    }
  }
}
