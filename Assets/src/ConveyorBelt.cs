using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {
  public Collider2D Collider2D;
  public Animator Animator;
  public float Speed;
  public bool Reverse;

  private List<GameObject> TouchingGameObjects = new();
  private MovementManager PlayerMovementManager;
  private bool MovedThisFixedUpdate = false;
  private float DeltaTime = 0;

  void Start() {
    PlayerMovementManager = PlayerGameObject.Current.GetComponent<MovementManager>();
  }

  void OnTriggerEnter2D(Collider2D collider) {
    TouchingGameObjects.Add(collider.gameObject);
  }

  void OnTriggerExit2D(Collider2D collider) {
    TouchingGameObjects.Remove(collider.gameObject);
  }

  /**
   * LateUpdate so that other scripts can adjust the speed for the current
   * frame.
   */
  void LateUpdate() {
    float realSpeed = Speed * (Reverse ? -1f : 1f);
    Animator.SetFloat("Speed", StateManager.Playing ? realSpeed : 0f);

    if (StateManager.Playing) {
      DeltaTime += Time.deltaTime;

      /**
       * Move each object at most once per fixed update, ensuring that the
       * logic preventing objects from overshooting works correctly. Changing
       * this method from Update to FixedUpdate doesn't work for unfathomable
       * reasons.
       */
      if (MovedThisFixedUpdate)
        return;

      MovedThisFixedUpdate = true;

      Vector2 direction = transform.TransformDirection(Vector2.down);
      Vector2 maxDisplacement = direction * realSpeed * DeltaTime;

      DeltaTime = 0;

      EligibleMovementManagers()
        .ForEach(movementManager => {
          Vector2 newPosition = movementManager.ConveyorContactPoint + maxDisplacement;

          // Do not overshoot by more than 0.01 units
          if (!InContact(newPosition)) {
            newPosition = (Vector2)Collider2D.bounds.ClosestPoint(newPosition) + direction * 0.01f;
          }

          Vector2 displacement = newPosition - movementManager.ConveyorContactPoint;

          movementManager.Move(displacement, skipVisualMovement: true, skipSpeedModifiers: true);
        });
    }
  }

  void FixedUpdate() {
    MovedThisFixedUpdate = false;
  }

  public bool PlayerInContact() => InContact(PlayerMovementManager.ConveyorContactPoint);

  private List<MovementManager> EligibleMovementManagers() =>
    TouchingGameObjects
      .Select(gameObject => gameObject.GetComponent<MovementManager>())
      .Where(movementManager =>
        movementManager != null && InContact(movementManager.ConveyorContactPoint)
      )
      .ToList();

  private bool InContact(Vector3 point) => Collider2D.bounds.Contains(point);
}
