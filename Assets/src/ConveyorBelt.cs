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

  void OnTriggerEnter2D(Collider2D collider) {
    TouchingGameObjects.Add(collider.gameObject);
  }

  void OnTriggerExit2D(Collider2D collider) {
    TouchingGameObjects.Remove(collider.gameObject);
  }

  void Update() {
    float realSpeed = Speed * (Reverse ? -1f : 1f);
    Animator.SetFloat("Speed", StateManager.Playing ? realSpeed : 0f);

    if (StateManager.Playing) {
      Vector2 velocity = transform.TransformDirection(Vector2.down * realSpeed);

      EligibleMovementManagers().ForEach(movementManager => {
        movementManager.Move(
          velocity * Time.deltaTime,
          skipVisualMovement: true,
          skipSpeedModifiers: true
        );
      });
    }
  }

  private List<MovementManager> EligibleMovementManagers() => TouchingGameObjects
    .Select(gameObject => gameObject.GetComponent<MovementManager>())
    .Where(movementManager => movementManager != null)
    .Where(InContact)
    .ToList();

  private bool InContact(MovementManager movementManager)
    => Collider2D.bounds.Contains(
      movementManager.transform.position +
      (Vector3)movementManager.ConveyorContactOffset
    );
}
