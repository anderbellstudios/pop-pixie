using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/**
 * The StateManager controls which state features are currently enabled for the
 * game. It's a singleton instance that persists across scene changes, although
 * the set of active states is reset to [Playing] whenever a scene is unloaded.
 *
 * There are two distinct concepts: states and state features.
 *
 * A state feature is some feature that is enabled in one or more states. Other
 * scripts can check if a particular state feature is enabled and adjust their
 * behaviour accordingly. For example, the AudioManager script checks if the
 * MuffleSounds state feature is enabled and applies a muffle audio effect if
 * so.
 *
 * States control which state features are currently enabled. Each state has a
 * set of state features that it enables and a set of state features that it
 * disables. States can also inherit from other states, which means they copy
 * all state features enabled or disabled by the parent state while adding
 * additional state feature changes of their own.
 *
 * Multiple states can be active at once, in which case the set of active state
 * features is determined by iterating over the list of active states (ordered
 * from earliest to latest enabled) and enabling or disabling the state features
 * prescribed by that state.
 *
 * To enact a state change, other scripts are expected to enable a new state
 * using StateManager.AddState (appending it to the list) and remove it using
 * StateManager.RemoveState when it's no longer needed.
 *
 * The Playing state is always enabled, although its effects can be overriden by
 * enabling additional states such as NotPlaying.
 *
 * Other scripts can listen for state changes using StateManager.AddListener and
 * check if a given state feature is enabled using StateManager.Enabled.
 */

[System.Flags]
public enum StateFeatures {
  None = 0,
  Playing = 1,
  BackgroundAnimations = 2,
  Movement = 4,
  PlayerDeathAnimation = 8,
  MuffleSounds = 16,
  PauseSounds = 32,
};

public enum State {
  Playing,
  NotPlaying,
  NotPlayingContinueSounds,
  ScriptedMovement,
  Paused,
  PlayerDying,
};

class StateDefinition {
  /** The state features enabled by this state. */
  public StateFeatures Enabled;

  /** The state features disabled by this state. */
  public StateFeatures Disabled;

  public StateDefinition(
    StateFeatures enable = StateFeatures.None,
    StateFeatures disable = StateFeatures.None
  ) {
    Enabled = enable;
    Disabled = disable;
  }

  /**
   * Create a new state definition based on this state.
   */
  public StateDefinition Extend(
    StateFeatures enable = StateFeatures.None,
    StateFeatures disable = StateFeatures.None
  ) => new StateDefinition(Enabled | enable, Disabled | disable);
}

public class StateManager : MonoBehaviour {
  public bool SingletonInstance = true;
  public static StateManager Current;

  /**
   * Playing state.
   *
   * This state is always enabled, although its state features (most notably the
   * Playing state feature) can be disabled by enabling additional states such
   * as NotPlaying.
   */
  static StateDefinition PlayingState = new StateDefinition(
    enable: StateFeatures.Playing | StateFeatures.BackgroundAnimations | StateFeatures.Movement
  );

  /**
   * NotPlayingContinueSounds state.
   *
   * Enabling this state halts all movement and gameplay but does not interrupt
   * sounds.
   */
  static StateDefinition NotPlayingContinueSoundsState = new StateDefinition(
    disable: StateFeatures.Playing | StateFeatures.Movement
  );

  /**
   * NotPlaying state.
   *
   * As NotPlayingContinueSoundsState, but also pauses any pausable sounds such
   * as dialogue and lengthy sound effects. Music is generally not paused.
   */
  static StateDefinition NotPlayingState = NotPlayingContinueSoundsState.Extend(
    enable: StateFeatures.PauseSounds
  );

  /**
   * ScriptedMovement state.
   *
   * This state halts gameplay without impeding movement. Use this state if you
   * want to move the player using code, such as during a cutscene.
   */
  static StateDefinition ScriptedMovementState = new StateDefinition(
    enable: StateFeatures.Movement,
    disable: StateFeatures.Playing
  );

  /**
   * Paused state.
   *
   * As NotPlaying, but also adds a muffle effect to sounds (most notably music)
   * and disables background animations. Use this state when you want to pause
   * absolutely everything, such as when the player enters the pause menu or the
   * weapon switcher.
   */
  static StateDefinition PausedState = NotPlayingState.Extend(
    enable: StateFeatures.MuffleSounds,
    disable: StateFeatures.BackgroundAnimations
  );

  /**
   * PlayerDying state.
   *
   * As NotPlaying, but also enables the death animation.
   */
  static StateDefinition PlayerDyingState = NotPlayingState.Extend(
    enable: StateFeatures.PlayerDeathAnimation
  );

  static StateDefinition GetStateDefinition(State state) {
    switch (state) {
      case State.Playing:
        return PlayingState;

      case State.NotPlaying:
        return NotPlayingState;

      case State.NotPlayingContinueSounds:
        return NotPlayingContinueSoundsState;

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

  public static void ResetStates(List<State> states) {
    EnhancedDataCollection.LogIfEnabled(() => "Resetting states");
    Current.ActiveStates = states.Select(GetStateDefinition).ToList();
    Current.HandleStateChanged();
  }

  public static void ResetStatesToDefault() {
    ResetStates(new List<State>() { State.Playing });
  }

  public static void AddState(State state) {
    EnhancedDataCollection.LogIfEnabled(() => "State added: " + state);
    Current.ActiveStates.Add(GetStateDefinition(state));
    Current.HandleStateChanged();
  }

  public static void RemoveState(State state) {
    EnhancedDataCollection.LogIfEnabled(() => "State removed: " + state);
    Current.ActiveStates.Remove(GetStateDefinition(state));
    Current.HandleStateChanged();
  }

  public static bool Enabled(StateFeatures flag) => Current.StateFeaturesCache.HasFlag(flag);

  public static bool Playing => Enabled(StateFeatures.Playing);

  public static void AddListener(UnityAction action) => Current.OnStateChanged.AddListener(action);

  public static void RemoveListener(UnityAction action) =>
    Current.OnStateChanged.RemoveListener(action);

  private List<StateDefinition> ActiveStates = new List<StateDefinition>();
  private StateFeatures StateFeaturesCache;
  private UnityEvent OnStateChanged = new UnityEvent();

  void Awake() {
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
