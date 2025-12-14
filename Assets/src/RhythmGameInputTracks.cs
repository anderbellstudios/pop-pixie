using System.Collections.Generic;
using UnityEngine;

public enum RhythmGameInputType {
  Left = 0,
  Down = 1,
  Up = 2,
  Right = 3,
};

public class RhythmGameInputTracks : MonoBehaviour {
  public float SpawnInputInterval,
    InputSpeed;
  public Transform LeftTarget,
    DownTarget,
    UpTarget,
    RightTarget;
  public GameObject ModelInput;

  private List<GameObject> SpawnedInputs = new();

  /**
   * The Y coordinate relative to this GameObject at which new inputs are
   * spawned.
   */
  private float SpawnY;

  void Start() {
    // Spawn inputs just below the bottom of the screen
    float inputSize = ModelInput.GetComponent<RectTransform>().rect.height;
    SpawnY = GetComponent<RectTransform>().rect.yMin - inputSize / 2f;

    AsyncTimer.BaseTime.SetInterval(SpawnInput, SpawnInputInterval, bindToGameObject: gameObject);
  }

  void Update() {
    Vector3 displacement = Vector2.up * InputSpeed * Time.deltaTime;

    SpawnedInputs.ForEach(input => {
      input.transform.localPosition += displacement;
    });
  }

  private void SpawnInput() {
    GameObject input = Instantiate(ModelInput, transform);
    input.SetActive(true);
    SpawnedInputs.Add(input);

    RhythmGameInputType inputType = (RhythmGameInputType)Random.Range(0, 4);
    Transform target = TargetForInputType(inputType);
    float spawnX = target.localPosition.x;

    input.transform.localPosition = new Vector2(spawnX, SpawnY);
    input.transform.localRotation = target.localRotation;
  }

  private Transform TargetForInputType(RhythmGameInputType inputType) {
    switch (inputType) {
      case RhythmGameInputType.Left:
        return LeftTarget;

      case RhythmGameInputType.Down:
        return DownTarget;

      case RhythmGameInputType.Up:
        return UpTarget;

      case RhythmGameInputType.Right:
        return RightTarget;
    }

    throw new System.ArgumentException("Unexpected input type");
  }
}
