using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum StateFeatures {
  None = 0,
  Playing = 1,
  BackgroundAnimations = 2,
  Movement = 4,
  InterruptSounds = 8,
  PlayerDeathAnimation = 16
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
    StateFeatures.InterruptSounds,
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

  static public void AddState(State state) {
    EnhancedDataCollection.LogIfEnabled(() => "State added: " + state);
    Current.ActiveStates.Add(StateTuple(state));
    Current.RecomputeStateFeaturesCache();
  }

  static public void RemoveState(State state) {
    EnhancedDataCollection.LogIfEnabled(() => "State removed: " + state);
    Current.ActiveStates.Remove(StateTuple(state));
    Current.RecomputeStateFeaturesCache();
  }

  static public bool Enabled(StateFeatures flag) {
    return Current.StateFeaturesCache.HasFlag(flag);
  }

  static public bool Playing => Enabled(StateFeatures.Playing);

  public List<State> InitialStates = new List<State> { State.Playing };

  protected List<(StateFeatures Enabled, StateFeatures Disabled)> ActiveStates = new List<(StateFeatures Enabled, StateFeatures Disabled)>();
  protected StateFeatures StateFeaturesCache;

  void Awake () {
    if (SingletonInstance)
      Current = this;

    InitialStates.ForEach(state => AddState(state));
  }

  protected void RecomputeStateFeaturesCache() {
    StateFeatures features = StateFeatures.None;

    ActiveStates.ForEach(state => {
      features &= ~state.Disabled;
      features |= state.Enabled;
    });

    StateFeaturesCache = features;
  }
}
