#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyAITest : ABaseTest {
  public static int Ticks = 0;
  private GameObject GameObject;

  [SetUp]
  public void Setup() {
    GameObject = new GameObject();
    Ticks = 0;
  }

  [TearDown]
  public void TearDown() {
    UnityEngine.GameObject.Destroy(GameObject);
  }

  private class MyRootAI : AMovementEnemyAI {
    public MyChildMovementAI ChildMovementAI;
    public MyChildGenericAI FirstChildAI, SecondChildAI;

    public int? ActivatedAt, DeactivatedAt;

    public void Start() {
      Activate();
    }

    public void Stop() {
      Deactivate();
    }

    protected override void OnActivate() {
      ActivatedAt = Ticks++;
    }

    protected override void OnDeactivate() {
      DeactivatedAt = Ticks++;
    }

    protected override AMovementEnemyAI UseMovementAI() => ChildMovementAI;

    protected override void UseChildAIs(System.Action<AGenericEnemyAI> useChild) {
      useChild(FirstChildAI);
      useChild(SecondChildAI);
    }
  }

  private class MyChildMovementAI : AMovementEnemyAI {
    public MyChildMovementAI ChildMovementAI;
    protected override AMovementEnemyAI UseMovementAI() => ChildMovementAI;
  }

  private class MyChildGenericAI : AGenericEnemyAI {
    public int? ActivatedAt, DeactivatedAt;

    protected override void OnActivate() {
      ActivatedAt = Ticks++;
    }

    protected override void OnDeactivate() {
      DeactivatedAt = Ticks++;
    }
  }

  [Test]
  public void MovementAIs() {
    MyRootAI rootAI = GameObject.AddComponent<MyRootAI>();
    MyChildMovementAI childMovement1 = GameObject.AddComponent<MyChildMovementAI>();
    MyChildMovementAI childMovement2 = GameObject.AddComponent<MyChildMovementAI>();
    MyChildMovementAI childMovement3 = GameObject.AddComponent<MyChildMovementAI>();

    rootAI.ChildMovementAI = childMovement1;
    childMovement1.ChildMovementAI = childMovement3;

    rootAI.Start();

    Assert.IsTrue(rootAI.IsActive);
    Assert.IsTrue(childMovement1.IsActive);
    Assert.IsFalse(childMovement2.IsActive);
    Assert.IsTrue(childMovement3.IsActive);

    rootAI.ChildMovementAI = childMovement2;
    rootAI.UpdateActiveChildAIs();

    Assert.IsTrue(rootAI.IsActive);
    Assert.IsFalse(childMovement1.IsActive);
    Assert.IsTrue(childMovement2.IsActive);
    Assert.IsFalse(childMovement3.IsActive);

    rootAI.Stop();

    Assert.IsFalse(rootAI.IsActive);
    Assert.IsFalse(childMovement1.IsActive);
    Assert.IsFalse(childMovement2.IsActive);
    Assert.IsFalse(childMovement3.IsActive);
  }

  [Test]
  public void GenericAIs() {
    MyRootAI rootAI = GameObject.AddComponent<MyRootAI>();
    MyChildGenericAI child1 = GameObject.AddComponent<MyChildGenericAI>();
    MyChildGenericAI child2 = GameObject.AddComponent<MyChildGenericAI>();

    rootAI.Start();

    Assert.IsTrue(rootAI.IsActive);
    Assert.IsFalse(child1.IsActive);
    Assert.IsFalse(child2.IsActive);

    rootAI.FirstChildAI = child1;
    rootAI.UpdateActiveChildAIs();

    Assert.IsTrue(rootAI.IsActive);
    Assert.IsTrue(child1.IsActive);
    Assert.IsFalse(child2.IsActive);

    rootAI.SecondChildAI = child2;
    rootAI.UpdateActiveChildAIs();

    Assert.IsTrue(rootAI.IsActive);
    Assert.IsTrue(child1.IsActive);
    Assert.IsTrue(child2.IsActive);

    rootAI.FirstChildAI = null;
    rootAI.UpdateActiveChildAIs();

    Assert.IsTrue(rootAI.IsActive);
    Assert.IsFalse(child1.IsActive);
    Assert.IsTrue(child2.IsActive);

    rootAI.SecondChildAI = null;
    rootAI.UpdateActiveChildAIs();

    Assert.IsTrue(rootAI.IsActive);
    Assert.IsFalse(child1.IsActive);
    Assert.IsFalse(child2.IsActive);
  }

  [Test]
  public void ActivationOrder() {
    MyRootAI rootAI = GameObject.AddComponent<MyRootAI>();
    MyChildGenericAI child1 = GameObject.AddComponent<MyChildGenericAI>();
    MyChildGenericAI child2 = GameObject.AddComponent<MyChildGenericAI>();

    rootAI.FirstChildAI = child1;
    rootAI.Start();

    rootAI.FirstChildAI = null;
    rootAI.SecondChildAI = child2;
    rootAI.UpdateActiveChildAIs();

    rootAI.Stop();

    Assert.AreEqual(0, rootAI.ActivatedAt, "Parent AI is activated first");
    Assert.AreEqual(1, child1.ActivatedAt);
    Assert.AreEqual(2, child1.DeactivatedAt, "Previous AI is deactivated before next AI is activated");
    Assert.AreEqual(3, child2.ActivatedAt);
    Assert.AreEqual(4, child2.DeactivatedAt, "Child AI is deactivated first");
    Assert.AreEqual(5, rootAI.DeactivatedAt);
  }
}
#endif
