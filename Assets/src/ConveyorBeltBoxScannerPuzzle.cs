using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ConveyorBeltBoxScannerPuzzle : MonoBehaviour {
  private enum TPuzzleState {
    Initial,
    Started,
    Detected,
  };

  public GenericInspectable StartPuzzleInspectable;
  public GameObject Box;
  public GameObject Scanner;
  public GameObject ElectricBarrier;
  public OnCollision BoxDestination;
  public float ConveyorBeltNormalSpeed,
    ConveyorBeltFastSpeed;
  public List<ConveyorBelt> ConveyorBelts;
  public UnityEvent OnResetScanner;

  private Vector3 BoxStartPosition;
  private TPuzzleState PuzzleState;

  void Start() {
    BoxStartPosition = Box.transform.position;

    StartPuzzleInspectable.OnInspectEvent.AddListener(StartPuzzle);

    // Reset the puzzle when the box reaches the end
    BoxDestination.OnCollide.AddListener(() => {
      if (BoxDestination.LastCollider.gameObject == Box) {
        InitializePuzzle();
      }
    });

    InitializePuzzle();
  }

  private void InitializePuzzle() {
    PuzzleState = TPuzzleState.Initial;
    SetConveyorBeltSpeed(0f);
    Box.SetActive(false);
    Scanner.SetActive(false);
    ElectricBarrier.SetActive(true);
    Box.transform.position = BoxStartPosition;
    OnResetScanner.Invoke();
    StartPuzzleInspectable.Inspectable = true;
  }

  private void StartPuzzle() {
    PuzzleState = TPuzzleState.Started;
    StartPuzzleInspectable.Inspectable = false;
    Box.SetActive(true);
    Scanner.SetActive(true);
    ElectricBarrier.SetActive(false);
    SetConveyorBeltSpeed(ConveyorBeltNormalSpeed);
  }

  public void OnPlayerDetected() {
    PuzzleState = TPuzzleState.Detected;
    ElectricBarrier.SetActive(true);
  }

  void Update() {
    /**
     * Speed up the conveyor belts when the player is detected, unless the
     * player is standing on a conveyor belt.
     */
    if (PuzzleState == TPuzzleState.Detected) {
      SetConveyorBeltSpeed(
        PlayerIsOnConveyorBelt() ? ConveyorBeltNormalSpeed : ConveyorBeltFastSpeed
      );
    }
  }

  private void SetConveyorBeltSpeed(float speed) {
    ConveyorBelts.ForEach(conveyorBelt => {
      conveyorBelt.Speed = speed;
    });
  }

  private bool PlayerIsOnConveyorBelt() =>
    ConveyorBelts.Any(conveyorBelt => conveyorBelt.PlayerInContact());
}
