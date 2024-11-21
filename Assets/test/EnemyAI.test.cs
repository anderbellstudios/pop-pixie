#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyAITest : ABaseTest {
  private class MyRootAI : AMovementEnemyAI {
    public MyChildMovementAI ChildMovementAI;

    public MyChildGenericAI FirstChildAI, SecondChildAI;

    public void Start() {
      Activate();
    }

    public void Stop() {
      Deactivate();
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
  }

  [Test]
  public void MovementAIs() {
    GameObject go = new GameObject();

    MyRootAI rootAI = go.AddComponent<MyRootAI>();
    MyChildMovementAI childMovement1 = go.AddComponent<MyChildMovementAI>();
    MyChildMovementAI childMovement2 = go.AddComponent<MyChildMovementAI>();
    MyChildMovementAI childMovement3 = go.AddComponent<MyChildMovementAI>();

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

    GameObject.Destroy(go);
  }

  [Test]
  public void GenericAIs() {
    GameObject go = new GameObject();

    MyRootAI rootAI = go.AddComponent<MyRootAI>();
    MyChildGenericAI child1 = go.AddComponent<MyChildGenericAI>();
    MyChildGenericAI child2 = go.AddComponent<MyChildGenericAI>();

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

    GameObject.Destroy(go);
  }
}
#endif
