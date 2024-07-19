#if UNITY_EDITOR
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
      Debug.LogFormat("PathfindingTest: Pass {0} of 5", i + 1);

      LoadSceneNotInBuildSettings("Assets/Unity/Scenes/Test Pathfinding.unity");
      yield return AwaitSceneChange("Test Pathfinding");

      GameObject agents = GameObject.Find("Agents");
      TestPathfindingBadZone badZone = GameObject.Find("BadZone").GetComponent<TestPathfindingBadZone>();
      TestPathfindingGoal split1 = GameObject.Find("Split1").GetComponent<TestPathfindingGoal>();
      TestPathfindingGoal split2 = GameObject.Find("Split2").GetComponent<TestPathfindingGoal>();

      foreach (Transform agent in agents.transform) {
        agent.gameObject.GetComponent<NavigateToPoint>().enabled = true;
      }

      yield return AwaitCondition(
        condition: () => {
          int remainingAgents = agents.transform.childCount;
          Debug.LogFormat("PathfindingTest: {0} remaining agents", remainingAgents);
          return remainingAgents == 0;
        },
        retries: 60,
        message: "All agents should reach their destination"
      );

      Assert.False(badZone.Failed, "Agent should not touch bad zone");

      Assert.NotZero(split1.Count, "Some agents should take split 1");
      Assert.NotZero(split2.Count, "Some agents should take split 2");

      SceneManager.LoadScene("Landing");
      yield return null;
    }
  }
}
#endif
