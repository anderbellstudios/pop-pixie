# Pop Pixie developer guide

## 1	File structure

C# files should be placed directly in `/Assets/src/`. The file name should be the same as the class name.

Other source files, such as images, scenes, prefabs and animations should be placed in their respective folders.

## 2	Code style

### 2.1	Linting

We use a fork of [CSharpier](https://csharpier.com/) for linting.

```
dotnet tool restore # First time only
yarn lint:fix # Automatically format *.cs files
```

### 2.2	Fields and properties

By default, use fields instead of properties, since they appear automatically in the Unity inspector.

Only use a property if you need to define a custom setter, although in most cases a separate setter method is preferred. For custom getters, arrow syntax is generally preferred.

All fields and properties should start with a capital letter.

When referencing another script, the field name should almost always be the same as the class name.

If a field should be editable in the Unity inspector, make it public. Otherwise, explicitly set it to private.

Public fields should only have default values if the script will be used in multiple scenes or prefabs. For scripts that are used in only one place, do not provide default values.

```cs
public class MyClass : MonoBehvaiour {
  public float Speed;
  public AnotherScript AnotherScript;
  
  private float SomePrivateData;
  private List<Whatever> Whatevers;
  
  // ...
}
```

### 2.3	Methods

`Awake`, `Start`, `Update` and other methods provided by Unity should usually appear before other methods. Do not use access modifiers for these methods.

Generally, `Start` should be preferred over `Awake`.

All other methods should usually be marked as `public`, `private`. Generally, public methods should appear before private methods. If method A calls method B, method A should generally come first.

All methods should start with a capital letter.

Methods that return a single expression should use arrow syntax. Consider removing the brackets for arrow methods with no parameters, so that they can be accessed like properties.

```cs
public class MyClass : MonoBehvaiour {
  void Awake() {}
  
  void Start() {}
  
  void Update() {}
  
  public void MethodOne() {}
  
  public void MethodTwo() {}
  
  private void MethodThree() {}
  
  private void MethodFour() {}
  
  private float Sum(float a, float b) => a + b;
  
  private GameObject Player => PlayerGameObject.Current;
}
```

### 	2.4	Referencing other scripts and GameObjects

Generally, scripts and GameObjects should be connected to one another using public fields in the Unity inspector.

```cs
public class MyClass : MonoBehvaiour {
  public MyOtherScript MyOtherScript;
}
```

To support polymorphism, reference the polymorphic script using an abstract class, which should start with `A` and inherit from `MonoBehaviour`.

```cs
public abstract class AEnemy : MonoBehaviour {
  public void SomeMethod();
}

public class MyEnemy : AEnemy {
  public override void SomeMethod() {
    // ...
  }
}

public class MyClass : MonoBehaviour {
  public AEnemy Enemy;
  
  void Start() {
    Enemy.SomeMethod();
  }
}
```

When referencing a script or GameObject that only appears once in a scene, and which is not part of the same prefab as the script you're referencing it from, use the singleton pattern described below.

For example, you can access the player's GameObject from any script using `PlayerGameObject.Current`.

#### 2.4.1	Singleton pattern

Define a public static field called `Current` and a public non-static boolean field `SingletonInstance` (defaulting to true) on the singleton class.

In the `Awake` method, assign the current instance to `Current` if `SingletonInstance` is true.

```cs
public void MySingleton : MonoBehaviour {
  public bool SingletonInstance = true;
  public static MySingleton Current;
  
  void Awake() {
    if (SingletonInstance)
      Current = this;
  }
}

// Anywhere in the scene:
MySingleton.Current
```

**Warning**: Do not try to access `MySingleton.Current` from the `Awake` method of another class. It will sometimes work, depending on the execution order of scripts in the scene, but it cannot be relied upon. Always access singleton objects from `Start` or `Update`. 

### 2.5	Comments

Comments less than 80 characters long should use single-line comments.

Comments longer than this should use JSDoc-style multi-line comments, where each line (including indentation) is less than 79 characters long.

All comments should start with a capital letter.

```cs
// Single-line comment

/**
 * Multi-line comment. Note that each line of the comment starts with a *, and
 * the first line has two *s.
 */
```

## 3	StateManager

`StateManager` is responsible for tracking whether the game is playing or paused. It also supports various other states, each with feature flags that determine whether things like background animations and sounds should continue to play.

The most common state feature you'll need to check for is `StateManager.Playing`. If a script should only run while the game is playing, check this state at the top of `Update`:

```cs
void Update() {
  if (!StateManager.Playing)
    return;
  
  // ...
}
```

## 4	Time, timers and stopwatches

In addition to the `Time.time` value provided by Unity, which tracks the number of seconds since the game started, Pop Pixie also has `PlayingTime.time`, which only increases while the game is in the playing state.

All behaviours that should be paused while the game is paused should use `PlayingTime`.

### 4.1	AsyncTimer

To call a function after a duration, use `AsyncTimer`, which is inspired by JavaScript's `setTimeout` and `setInterval` functions.

`AsyncTimer.BaseTime.SetTimeout(callback, delay)` calls `callback` after `delay` seconds.

`AsyncTimer.BaseTime.SetInterval(callback, delay)` calls `callback` every `delay` seconds.

You can replace `BaseTime` with `PlayingTime` to use playing time instead.

Each of these methods returns an `AsyncTimer.EnqueuedEvent` object, which can be cancelled using `AsyncTimer.BaseTime.ClearTimeout`.

To cancel a timer automatically if a behaviour is disabled, use:

```cs
AsyncTimer.BaseTime.SetTimeout(callback, delay, bindToBehaviour: this);
```

To cancel a timer automatically if a GameObject is destroyed, use:

```cs
AsyncTimer.BaseTime.SetTimeout(callback, delay, bindToGameObject: gameObject);
```

### 4.2	Stopwatch

To track the time that has passed since a point in time, use `Stopwatch`.

To create a stopwatch, use `new Stopwatch.BaseTime()` or `new Stopwatch.PlayingTime()`.

A stopwatch records the current time at the moment when it's created. To reset a stopwatch to the current time, use `MyStopwatch.Reset()`.

The most common methods you can call on a stopwatch are:

- `MyStopwatch.Time()` - Returns the time in seconds since the stopwatch was started.
- `MyStopwatch.Progress(duration)` - Returns a float between 0 and 1 representing the fraction of `duration` that has elapsed.
