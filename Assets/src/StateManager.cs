using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Flags]
public enum StateFeatures {
  None = 0,
  Playing = 1,
  BackgroundAnimations = 2,
  Movement = 4,
  PauseAudio = 8,
  PlayerDeathAnimation = 16,
  MuffleSounds = 32
};

public enum State {
  Playing,
  NotPlaying,
  ScriptedMovement,
  Paused,
  PlayerDying
};

public class StateManager : MonoBehaviour {
  public bool SingletonInstance = true;
  public static StateManager Current;

  static (StateFeatures Enabled, StateFeatures Disabled) PlayingState = (
    StateFeatures.Playing | StateFeatures.BackgroundAnimations | StateFeatures.Movement,
    StateFeatures.None
  );

  static (StateFeatures Enabled, StateFeatures Disabled) NotPlayingState = (
    StateFeatures.None,
    StateFeatures.Playing | StateFeatures.Movement
  );

  static (StateFeatures Enabled, StateFeatures Disabled) ScriptedMovementState = (
    StateFeatures.Movement,
    StateFeatures.Playing
  );

  static (StateFeatures Enabled, StateFeatures Disabled) PausedState = InheritFrom(NotPlayingState, (
    StateFeatures.MuffleSounds,
    StateFeatures.BackgroundAnimations
  ));

  static (StateFeatures Enabled, StateFeatures Disabled) PlayerDyingState = InheritFrom(NotPlayingState, (
    StateFeatures.PlayerDeathAnimation,
    StateFeatures.None
  ));

  static (StateFeatures Enabled, StateFeatures Disabled) StateTuple(State state) {
    switch (state) {
      case State.Playing:
        return PlayingState;

      case State.NotPlaying:
        return NotPlayingState;

      case State.ScriptedMovement:
        return ScriptedMovementState;

      case State.Paused:
        return PausedState;

      case State.PlayerDying:
        return PlayerDyingState;

      default:
        throw new System.ArgumentException("Unknown state");
    }
  }

  static (StateFeatures Enabled, StateFeatures Disabled) InheritFrom((StateFeatures Enabled, StateFeatures Disabled) a, (StateFeatures Enabled, StateFeatures Disabled) b)
    => (a.Enabled | b.Enabled, a.Disabled | b.Disabled);

  static public void ResetStates(List<State> states) {
    EnhancedDataCollection.LogIfEnabled(() => "Resetting states");
    Current.ActiveStates = states.Select(StateTuple).ToList();
    Current.HandleStateChanged();
  }

  static public void ResetStatesToDefault() {
    ResetStates(new List<State>() { State.Playing });
  }

  static public void AddState(State state) {
    EnhancedDataCollection.LogIfEnabled(() => "State added: " + state);
    Current.ActiveStates.Add(StateTuple(state));
    Current.HandleStateChanged();
  }

  static public void RemoveState(State state) {
    EnhancedDataCollection.LogIfEnabled(() => "State removed: " + state);
    Current.ActiveStates.Remove(StateTuple(state));
    Current.HandleStateChanged();
  }

  static public bool Enabled(StateFeatures flag) {
    return Current.StateFeaturesCache.HasFlag(flag);
  }

  static public bool Playing => Enabled(StateFeatures.Playing);

  static public void AddListener(UnityAction action) => Current.OnStateChanged.AddListener(action);
  static public void RemoveListener(UnityAction action) => Current.OnStateChanged.RemoveListener(action);

  protected List<(StateFeatures Enabled, StateFeatures Disabled)> ActiveStates = new List<(StateFeatures Enabled, StateFeatures Disabled)>();
  protected StateFeatures StateFeaturesCache;
  protected UnityEvent OnStateChanged = new UnityEvent();

  void Awake () {
    if (SingletonInstance) {
      if (Current != null) {
        Destroy(gameObject);
        return;
      }

      Current = this;
      DontDestroyOnLoad(gameObject);

      ResetStatesToDefault();
      SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
  }

  void OnSceneUnloaded(Scene scene) {
    ResetStatesToDefault();
  }

  protected void HandleStateChanged() {
    StateFeatures features = StateFeatures.None;

    ActiveStates.ForEach(state => {
      features &= ~state.Disabled;
      features |= state.Enabled;
    });

    StateFeaturesCache = features;

    OnStateChanged.Invoke();
  }
}
