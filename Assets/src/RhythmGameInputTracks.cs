using System;
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

  private LinearWindow<RhythmGameInput> VisibleInputsWindow;
  private Func<float> GetTime;
  private Dictionary<RhythmGameInput, GameObject> InputGameObjects = new();

  private bool AfterFirstUpdate = false;
  private float TargetY;
  private float SpawnY;
  private float DespawnY;
  private float InputSpeed;

  public void Init(RhythmGameSong song, Func<float> getTime) {
    VisibleInputsWindow = new(song.Inputs, onEnterWindow: SpawnInput, onExitWindow: DespawnInput);
    GetTime = getTime;

    TargetY = transform.InverseTransformPoint(LeftTarget.position).y;

    float inputSize = ModelInput.GetComponent<RectTransform>().rect.height;
    float bottomOfScreen = GetComponent<RectTransform>().rect.yMin;
    float topOfScreen = GetComponent<RectTransform>().rect.yMax;
    SpawnY = bottomOfScreen - inputSize / 2f - SPAWN_DESPAWN_LEEWAY;
    DespawnY = topOfScreen + inputSize / 2f + SPAWN_DESPAWN_LEEWAY;

    float secondsPerBeat = 60f / song.BeatsPerMinute;
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

    VisibleInputsWindow.Update(input => {
      float y = YPositionForInput(input);
      return y >= SpawnY && y <= DespawnY;
    });

    foreach (RhythmGameInput input in VisibleInputs) {
      if (InputGameObjects.ContainsKey(input)) {
        float y = YPositionForInput(input);
        Transform inputTransform = InputGameObjects[input].transform;
        inputTransform.localPosition = new Vector2(inputTransform.localPosition.x, y);
      }
    }
  }

  public void HitInput(RhythmGameInput input) {
    DespawnInput(input);
    VisibleInputsWindow.RemoveEarly(input);
  }

  private float YPositionForInput(RhythmGameInput input) {
    float distanceToTarget = input.RelativeTime(CurrentTime) * InputSpeed;
    return TargetY - distanceToTarget;
  }

  private void SpawnInput(RhythmGameInput input) {
    GameObject inputGameObject = Instantiate(ModelInput, transform);
    inputGameObject.SetActive(true);
    InputGameObjects.Add(input, inputGameObject);

    Transform target = TargetForInputType(input.Type);
    float spawnX = transform.InverseTransformPoint(target.position).x;

    inputGameObject.transform.localPosition = new Vector2(spawnX, SpawnY);
    inputGameObject.transform.localRotation = target.localRotation;
    inputGameObject.GetComponent<Image>().color = target.GetComponent<Image>().color;
  }

  private void DespawnInput(RhythmGameInput input) {
    if (InputGameObjects.ContainsKey(input)) {
      Destroy(InputGameObjects[input]);
      InputGameObjects.Remove(input);
    }
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

  public List<RhythmGameInput> VisibleInputs => VisibleInputsWindow.Current;
  private float CurrentTime => GetTime();
}
