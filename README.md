# Pop Pixie

[**🛠 Under active development**](#project-status)

In this top-down, story-driven shooter, you play as Pop Pixie, the rightful inheritor of the Pop™ brand of soda. Your mission is to infiltrate the headquarters of the evil Mentoes Corporation, avenge the death of your father, and bring the company’s dark secrets to light.

Mr Mentoe and his minions will do everything in their power to put a stop to your mission. Fight back with an expansive arsenal of fizzing firearms and soda-themed weapons.

But you can’t do it alone. Masterminding your mission is Amanda, who will stop at nothing to put an end to the corrupt corporatocracies that carelessly destroy families like hers every day.

## Gameplay

- Fight your way through swarms of bloodthirsty gremlins
- Outsmart and outmanoeuvre ruthless bosses
- Unlock an expansive arsenal of fizzing firearms and soda-themed weapons
- Learn the dark secrets behind the Mentoes Corporation

## Project Status

**Update January 2024**: We're putting the finishing touches to the supplementary prequel game *Pop Pixie: Mission Training*, which we're hoping to release in 2024. After that, we'll work on getting the first few levels of the main game ready to play and start releasing incremental updates.

Follow us on social media for more updates:

- [Twitter](https://twitter.com/AnderbellStds)
- ~Discord~ (coming soon)
- ~Website~ (coming soon)

## Development

Follow these instructions to run Pop Pixie locally.

### Install Unity

1. Install the latest version of [Unity Hub](https://unity.com/download)
2. Inside Unity Hub, click Open and select Pop Pixie
3. Follow the instructions to install the version of Unity Editor currently used by Pop Pixie
   - When prompted to add modules, if you wish to compile standalone builds of Pop Pixie, select the following:
     - Mac Build Support (IL2CCP)
     - Windows Build Support (Mono)
4. When you open Pop Pixie, there will be compile errors. Click Ignore when Unity prompts you about these to open the Unity editor.

### Add dependencies

Due to incompatible licensing, some components and assets required by Pop Pixie cannot be included in this GitHub repo. These dependencies should be acquired separately and placed in the `/Assets/vendor/` directory.

To check which dependencies are currently missing, run `ruby Assets/vendor/validate.rb`.

When adding a dependency purchased through the Asset Store, import it using Unity's Package Manager under My Assets. Generally, it's safe to uncheck any directories labelled "Documentation" and "Examples", and including these in the import may increase compile time.

Unity will automatically place these dependencies in `/Assets`, but you should move them into `/Assets/vendor` to ensure they aren't committed to Git. We will reject any PR that includes non-MIT licensable dependencies.

#### Rewired (Asset Store, paid, free trial available)

We use Rewired for handling user input in Pop Pixie. Without it, the game will not compile and keyboard, mouse and controller input will not work. The free trial stops working after five minutes of runtime, which is the only limitation.

Purchase from https://guavaman.com/projects/rewired/

Move from `/Assets/Rewired` to `/Assets/vendor/Rewired`

**When there are no more compile errors, Rewired will prompt you to begin installation. Please skip this step.**

#### Retro: CRT TV (Asset Store, paid)

We use Retro: CRT TV for the CRT effect in Pop Pixie. Without it, Access Terminal screens won't render correctly, but will still be legible.

Purchase from https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/retro-crt-tv-241411

Move from `/Assets/FronkonGames` to `/Assets/vendor/FronkonGames`

#### Pixel Art City Backgrounds (third-party, paid)

The city background used in some scenes in Pop Pixie is modified from the Pixel Art City Backgrounds asset pack by edermunizz. Due to the license under which this asset pack is sold, we're unable to include our modified version of the city background in this GitHub repo. The game will still run without the background, but some scenes will be rendered incorrectly.

Purchase from https://edermunizz.itch.io/pixel-art-city-backgrounds. If you send us proof that you have done so, we will give you a copy of the modified background.

Expected file structure:

```
/Assets/vendor/Main Menu Background
├── Lights 1.png
├── Lights 2.png
├── Lights 3.png
├── Lights 4.png
├── Lights 5.png
├── Lights 6.png
├── Lights 7.png
├── Lights 8.png
├── Lights 9.png
├── Lights 10.png
├── Main Menu Background.png
└── Tower M.png
```

#### TextMesh Pro (special, free)

We use TextMesh Pro for rendering text in Pop Pixie.

Import by selecting TextMeshPro > Import TMP Essential Resources

Move from `/Assets/TextMesh Pro` to `/Assets/vendor/TextMesh Pro`

#### DOTween (third-party, free)

DOTween is a dependency of TMP_Typewriter, which we'll be installing shortly. Since DOTween's website does not support HTTPS and so downloading executable code from there carries significant risk, we provide a [compiled mirror](https://github.com/anderbellstudios/DOTween) of the version of DOTween used by Pop Pixie.

Add by running `git clone https://github.com/anderbellstudios/DOTween Assets/vendor/DOTween` in the root of the project

#### TMP_Typewriter (GitHub, free)

We use TMP_Typewriter for the typewriter effect used in Pop Pixie's dialogue box and other places.

Download `TMP_Typewriter_v1.0.0.unitypackage` from https://github.com/baba-s/TMP_Typewriter/releases and import using Unity

Move from `/Assets/TMP_Typewriter` to `/Assets/vendor/TMP_Typewriter`

#### JsonDotNet (GitHub, free)

We use JsonDotNet (otherwise known as Newtonsoft.Json) for various internal subsystems of Pop Pixie, mainly save data storage and data collection.

Download from https://github.com/JamesNK/Newtonsoft.Json/releases

If there are compiled binaries for multiple .NET versions, use the latest 4.x version.

Place any assemblies in `/Assets/vendor/JsonDotNet`

#### CrossSceneReference (GitHub, free)

We use CrossSceneReference as part of Pop Pixie's scene serialisation system (soon to be deprecated).

Add by running the following in the root of the project:

```bash
git clone https://github.com/Unity-Technologies/guid-based-reference
mv guid-based-reference/Assets/CrossSceneReference Assets/vendor/CrossSceneReference
rm -rf guid-based-reference
rm -r Assets/vendor/CrossSceneReference/{Tests,Documentation~,Samples}
```

#### Summary

If you installed all of those dependencies correctly, `ruby Assets/vendor/validate.rb` should have no output and there should be no compile errors in the Unity console. (It's safe to ignore any warnings.)

### Check that the repo is clean

Unity may have modified `ProjectSettings/ProjectSettings.asset` when starting up Unity. Please revert these changes using `git checkout -- ProjectSettings/ProjectSettings.asset`.

At this point, `git status` should report that the working tree is clean. Please try to resolve any inconsistencies if this is not the case.

### Set the environment

Pop Pixie has various environments (separate versions of Unity config files) and provides a script to switch between them. Run the following in the root of the project to set the environment to `develop`:

```bash
gem install tty-prompt # First time only
ruby setenv.rb
```

### What's next?

You should now be ready to begin work on Pop Pixie. Open a Unity scene from `/Assets/Unity/Scenes` to get started.
