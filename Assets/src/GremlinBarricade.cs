using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GremlinBarricade : MonoBehaviour {
  public GameObject FallingDebris;
  public float XRange, MinY, MaxY;
  public float MinInterval, MaxInterval;
  public float MinDistanceBetweenDebris;

  private List<GameObject> ThrownObjects = new();

  void Start() {
    ScheduleThrow();
  }

  private void ScheduleThrow() {
    AsyncTimer.PlayingTime.SetTimeout(() => {
      Throw();
      ScheduleThrow();
    }, Random.Range(MinInterval, MaxInterval));
  }

  private void Throw() {
    CleanUpThrownObjects();

    Vector3 position = RandomPosition();

    GameObject thrownObject = Instantiate(
      FallingDebris,
      position,
      Quaternion.identity
    );

    FallingDebris fallingDebris = thrownObject.GetComponent<FallingDebris>();
    fallingDebris.Distance = transform.position.y - position.y;
    fallingDebris.CounterAttackTarget = transform.position;

    ThrownObjects.Add(thrownObject);
  }

  private void CleanUpThrownObjects() {
    ThrownObjects.RemoveAll(thrownObject => thrownObject == null);
  }

  private Vector3 RandomPosition() {
    Vector3 position = Vector3.zero;

    for (int attempts = 30; attempts >= 0; attempts--) {
      position = transform.position + new Vector3(
        Random.Range(-XRange, XRange),
        -Random.Range(MinY, MaxY)
      );

      if (PositionIsGood(position))
        return position;
    }

    return position;
  }

  private bool PositionIsGood(Vector3 position) =>
    ThrownObjects.All(thrownObject => (
      thrownObject.transform.position - position
    ).magnitude >= MinDistanceBetweenDebris);
}
