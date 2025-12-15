using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmGameInputTracks : MonoBehaviour {
  public float PixelsBetweenEachBeat;
  public Transform LeftTarget;
  public Transform DownTarget;
  public Transform UpTarget;
  public Transform RightTarget;
  public GameObject ModelInput;

  /**
   * The number of pixels added or subtracted to SpawnY and DespawnY to ensure
   * that inputs are spawned well before they're due to appear on the screen,
   * and have time to be marked as misses before they're despawned.
   */
  private const float SPAWN_DESPAWN_LEEWAY = 1080f;

  private RhythmGame RhythmGame;
  private bool AfterFirstUpdate = false;
  public List<SpawnedInput> SpawnedInputs { get; private set; } = new();
  private float TargetY;
  private float SpawnY;
  private float DespawnY;
  private float InputSpeed;
  private int LastSpawnedIndex = -1;

  public void Init(RhythmGame rhythmGame) {
    RhythmGame = rhythmGame;

    TargetY = transform.InverseTransformPoint(LeftTarget.position).y;

    float inputSize = ModelInput.GetComponent<RectTransform>().rect.height;
    float bottomOfScreen = GetComponent<RectTransform>().rect.yMin;
    float topOfScreen = GetComponent<RectTransform>().rect.yMax;
    SpawnY = bottomOfScreen - inputSize / 2f - SPAWN_DESPAWN_LEEWAY;
    DespawnY = topOfScreen + inputSize / 2f + SPAWN_DESPAWN_LEEWAY;

    float secondsPerBeat = 60f / Song.BeatsPerMinute;
    InputSpeed = PixelsBetweenEachBeat / secondsPerBeat;
  }

  void Update() {
    /**
     * Do not spawn any inputs during the first update, since the positions can
     * be incorrect.
     */
    if (!AfterFirstUpdate) {
      AfterFirstUpdate = true;
      return;
    }

    List<SpawnedInput> toDespawn = new();

    // Update spawned inputs
    foreach (SpawnedInput spawnedInput in SpawnedInputs) {
      float y = YPositionForInput(spawnedInput.Input);

      if (y >= DespawnY) {
        toDespawn.Add(spawnedInput);
      } else {
        Transform inputTransform = spawnedInput.GameObject.transform;
        inputTransform.localPosition = new Vector2(inputTransform.localPosition.x, y);
      }
    }

    // Despawn off-screen inputs
    toDespawn.ForEach(DespawnInput);

    // Spawn new inputs
    while (LastSpawnedIndex < Inputs.Count - 1) {
      RhythmGameInput nextInput = Inputs[LastSpawnedIndex + 1];

      if (DueToSpawnInput(nextInput)) {
        SpawnInput(nextInput);
        LastSpawnedIndex++;
      } else {
        break;
      }
    }
  }

  public void HitInput(RhythmGameInput input) {
    SpawnedInput spawnedInput = SpawnedInputs.Find(spawnedInput => spawnedInput.Input == input);
    DespawnInput(spawnedInput);
  }

  private float YPositionForInput(RhythmGameInput input) {
    float distanceToTarget = input.RelativeTime(CurrentTime) * InputSpeed;
    return TargetY - distanceToTarget;
  }

  private bool DueToSpawnInput(RhythmGameInput input) => YPositionForInput(input) >= SpawnY;

  private void SpawnInput(RhythmGameInput input) {
    GameObject inputGameObject = Instantiate(ModelInput, transform);
    inputGameObject.SetActive(true);

    Transform target = TargetForInputType(input.Type);
    float spawnX = transform.InverseTransformPoint(target.position).x;

    inputGameObject.transform.localPosition = new Vector2(spawnX, SpawnY);
    inputGameObject.transform.localRotation = target.localRotation;
    inputGameObject.GetComponent<Image>().color = target.GetComponent<Image>().color;

    SpawnedInput spawnedInput = new SpawnedInput { Input = input, GameObject = inputGameObject };
    SpawnedInputs.Add(spawnedInput);
  }

  void DespawnInput(SpawnedInput spawnedInput) {
    Destroy(spawnedInput.GameObject);
    SpawnedInputs.Remove(spawnedInput);
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

  private RhythmGameSong Song => RhythmGame.Song;
  private List<RhythmGameInput> Inputs => Song.Inputs;
  private float CurrentTime => RhythmGame.CurrentTime;

  public class SpawnedInput {
    public RhythmGameInput Input;
    public GameObject GameObject;
  }
}
