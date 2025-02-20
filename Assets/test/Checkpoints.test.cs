#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CheckpointsTest : ABaseTest {
  private IEnumerator Setup() {
    yield return CommonSetup();
    LoadSceneNotInBuildSettings("Assets/Unity/Scenes/Test Checkpoints.unity");
    yield return AwaitSceneChange("Test Checkpoints");
  }

  [UnityTest, Retry(3)]
  public IEnumerator ResumeWithoutCheckpoint() {
    yield return Setup();

    yield return DieAndResume();
    AssertPlayerPosition(GetAnchorPosition("Initial"));

    AssertPileOfCans(false);
    AssertEnemyAlive();
    AssertKeycard(false);
    AssertAccessTerminal(false);
    AssertRingPull(false);
    AssertIntel(false);
  }

  [UnityTest, Retry(3)]
  public IEnumerator ResumeFromLeftCheckpoint() {
    yield return Setup();

    yield return ScriptedMovement("LeftCheckpoint");
    yield return DieAndResume();
    AssertPlayerPosition(GetAnchorPosition("LeftCheckpoint"));

    AssertPileOfCans(false);
    AssertEnemyAlive();
    AssertKeycard(false);
    AssertAccessTerminal(false);
    AssertRingPull(false);
    AssertIntel(false);
  }

  [UnityTest, Retry(3)]
  public IEnumerator ResumeFromRightCheckpoint() {
    yield return Setup();

    yield return ScriptedMovement("RightCheckpoint");
    yield return DieAndResume();
    AssertPlayerPosition(GetAnchorPosition("RightCheckpoint"));

    AssertPileOfCans(false);
    AssertEnemyAlive();
    AssertKeycard(false);
    AssertAccessTerminal(false);
    AssertRingPull(false);
    AssertIntel(false);
  }

  [UnityTest, Retry(3)]
  public IEnumerator PileOfCansStaysDestroyed() {
    yield return Setup();

    PileOfCansHitPoints.Damage(1);
    yield return ScriptedMovement("LeftCheckpoint");
    yield return DieAndResume();
    AssertPileOfCans(true);
  }

  [UnityTest, Retry(3)]
  public IEnumerator EnemyRevivesAndKeycardStaysGot() {
    yield return Setup();

    yield return KillAllEnemiesAndAwaitKeycard();

    yield return ScriptedMovement("LeftCheckpoint");
    yield return DieAndResume();

    AssertEnemyAlive();
    AssertKeycard(true);
  }

  [UnityTest, Retry(3)]
  public IEnumerator AccessTerminalStaysUsed() {
    yield return Setup();

    yield return KillAllEnemiesAndAwaitKeycard();
    yield return ScriptedMovement("Terminal");
    yield return PressButton("Inspect");
    yield return PressButton("Pause"); // Skip Access Terminal
    yield return PressButton("Cancel"); // Close lore window

    yield return ScriptedMovement("LeftCheckpoint");
    yield return DieAndResume();

    AssertKeycard(true);
    AssertAccessTerminal(true);
  }

  [UnityTest, Retry(3)]
  public IEnumerator RingPullStaysGot() {
    yield return Setup();

    yield return ScriptedMovement("RingPull");
    yield return ScriptedMovement("LeftCheckpoint");
    yield return DieAndResume();
    AssertRingPull(true);
  }

  [UnityTest, Retry(3)]
  public IEnumerator IntelStaysGot() {
    yield return Setup();

    yield return ScriptedMovement("Intel");
    yield return PressButton("Inspect");
    yield return PressButton("Cancel"); // Close lore window
    yield return ScriptedMovement("LeftCheckpoint");
    yield return DieAndResume();
    AssertIntel(true);
  }

  [UnityTest, Retry(3)]
  public IEnumerator UseMultipleCheckpoints() {
    yield return Setup();

    yield return KillAllEnemiesAndAwaitKeycard();

    yield return ScriptedMovement("LeftCheckpoint");
    yield return DieAndResume();
    AssertPlayerPosition(GetAnchorPosition("LeftCheckpoint"));

    AssertKeycard(true);
    AssertPileOfCans(false);

    PileOfCansHitPoints.Damage(1);

    yield return ScriptedMovement("RightCheckpoint");
    yield return DieAndResume();
    AssertPlayerPosition(GetAnchorPosition("RightCheckpoint"));

    AssertKeycard(true);
    AssertPileOfCans(true);
  }

  [UnityTest, Retry(3)]
  public IEnumerator UseCheckpointTwice() {
    yield return Setup();

    yield return KillAllEnemiesAndAwaitKeycard();

    yield return ScriptedMovement("LeftCheckpoint");
    yield return DieAndResume();
    AssertPlayerPosition(GetAnchorPosition("LeftCheckpoint"));

    AssertKeycard(true);
    AssertPileOfCans(false);

    yield return ScriptedMovement("Initial");
    PileOfCansHitPoints.Damage(1);

    yield return ScriptedMovement("LeftCheckpoint");
    yield return DieAndResume();
    AssertPlayerPosition(GetAnchorPosition("LeftCheckpoint"));

    AssertKeycard(true);
    AssertPileOfCans(true);
  }

  private GameObject PileOfCans => GameObject.Find("Pile of Cans");
  private HitPoints PileOfCansHitPoints => PileOfCans.GetComponent<HitPoints>();

  private void AssertPileOfCans(bool destroyed) {
    Assert.AreEqual(destroyed, PileOfCansHitPoints.Dead, "Pile of Cans is destroyed");
  }

  private GameObject Enemy => GameObject.Find("Enemy");
  private HitPoints EnemyHitPoints => Enemy.GetComponent<HitPoints>();

  private void AssertEnemyAlive() {
    Assert.NotNull(Enemy, "Enemy is not null");
    Assert.IsFalse(EnemyHitPoints.Dead, "Enemy is not dead");
  }

  private void AssertKeycard(bool got) {
    Assert.AreEqual(got, LevelObjectives.GotKeycard, "Got keycard");
  }

  private void AssertAccessTerminal(bool used) {
    Assert.AreEqual(used, LevelObjectives.UsedAccessTerminal, "Used Access Terminal");
  }

  private void AssertRingPull(bool got) {
    bool gameObjectDestroyed = GameObject.Find("Floating Ring Pull") == null;
    Assert.AreEqual(got, gameObjectDestroyed, "Got ring pull");
  }

  private void AssertIntel(bool got) {
    bool collected = GameObject.Find("Piece of Intel").GetComponent<PieceOfIntelSprite>().Collected;
    Assert.AreEqual(got, collected, "Got Piece of Intel");
  }
}
#endif
