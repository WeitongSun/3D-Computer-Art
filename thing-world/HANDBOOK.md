# Thing World — Student Handbook

*3D Computer Art · ShanMu Sun* · Inspired by [*ThingThingThing*](https://github.com/ZZYW/ThingThingThing) by ZZYW

---

## What we're making

Together, the class builds **one world** that runs in a Meta Quest 3 headset.

You contribute two things:

- **Your Thing** — your self-portrait character from Blender, brought to life with a few lines of code. Once the world starts, nobody controls it. It walks around, meets other people's Things, reacts to sunrise and sunset, and responds when the visitor comes near or picks it up.
- **Your pieces of the city** — buildings, objects, landscapes. The class arranges everyone's pieces into one collaged city.

You don't need to know how to program. The hard parts (walking, physics, being grabbed and thrown, standing back up, speech bubbles, sound, animation) are done for you. You decide **what happens when**.

---

## Contents

1. [Your folder — the golden rule](#1-your-folder--the-golden-rule)
2. [Bring your character into Unity](#2-bring-your-character-into-unity)
3. [Write your Thing's behavior](#3-write-your-things-behavior)
4. [Reference: settings, events, actions](#4-reference)
5. [Recipes](#5-recipes)
6. [Test it](#6-test-it)
7. [Build your city pieces](#7-build-your-city-pieces)
8. [Rules for a world that runs smoothly](#rules-for-a-world-that-runs-smoothly)
9. [Check and export your work](#9-check-and-export-your-work)
10. [Animation (and motion capture later)](#10-animation-and-motion-capture-later)
11. [When something goes wrong](#11-when-something-goes-wrong)

---

## 1. Your folder — the golden rule

> **Everything you make lives inside `Assets/Creators/YourName`. Never edit anything outside it.**

Your folder looks like this (Thing World creates it for you):

```
Assets/Creators/YourName/
├── Character/            your character FBX and its textures
├── Scripts/              YourNameThing.cs  ← your code
├── Resources/Things/     YourNameThing.prefab  ← your finished Thing (made for you)
├── Animation/            YourName_Animations  ← which animation plays when
├── Pieces/               your city pieces (prefabs + their models and textures)
├── Sounds/               your audio files
└── YourName_TestScene    your private test world
```

When you hand in your work, Thing World packs **only this folder**. Anything you put somewhere else will be missing in the class world.

Don't touch `Assets/ThingWorld` (the framework) or `Assets/Samples` (the VR toolkit). Everybody shares those.

---

## 2. Bring your character into Unity

### 2.1 Export from Blender

1. Make sure your character stands on the ground (feet at Z = 0), faces **−Y** (Blender's front view), and is human-sized (about 1.7 m).
2. Select the character → **Object → Apply → All Transforms** (Ctrl/Cmd + A).
3. Keep it light: **15,000 triangles or fewer**, **4 materials or fewer**. Check with *Viewport Overlays → Statistics*. Use a **Decimate** modifier if needed.
4. **File → Export → FBX**. Tick *Selected Objects*, set *Apply Scalings* to **FBX All**, *Forward* **−Z Forward**, *Up* **Y Up**. Under *Path Mode* choose **Copy** and press the small box icon next to it (**Embed Textures**).

### 2.2 Rig it with Mixamo (so it can walk)

1. Go to [mixamo.com](https://www.mixamo.com) (free Adobe account) → **Upload Character** → your FBX.
2. Place the markers (chin, wrists, elbows, knees, groin) → let it auto-rig.
3. Download your character: **Format FBX for Unity**, **Skin: With Skin**, **Pose: T-pose**.
4. Download two animations for now:
   - an **Idle** animation
   - a **Walking** animation **with "In Place" ticked** (important — Thing World moves the body; the animation should walk on the spot)

   For animations choose **FBX for Unity**, **Without Skin**.

> No time to rig? A static character works too — it will glide instead of walk. You can add a rig later and press *Update My Thing*.

### 2.3 Import into Unity

1. In Unity: **Thing World → 1. Create or Update My Thing**. Type your first name and click **Make My Folders**.
2. Drag your character FBX into `Assets/Creators/YourName/Character` in the Project window. Drag the animation FBX files there too.
3. For each **animation** FBX: click it → Inspector → **Rig** tab → *Animation Type* **Humanoid** → **Apply**. Then the **Animation** tab → tick **Loop Time** → **Apply**.
4. Back in the *Create My Thing* window: drag your character into **My Character**. Open the little triangle on an animation FBX in the Project window to find the clip inside (the triangle-play icon) and drag it into **Idle animation** / **Walk animation**.
5. Click **Create My Thing**. If it asks to make your character **Humanoid**, say yes. If it says your character is 170 m tall, let it resize.

Unity compiles for a moment, then opens **your test scene**, with your Thing standing in front of the player and three grey Blobs to meet. Your script is at `Assets/Creators/YourName/Scripts/YourNameThing.cs` — double-click it to open it in your code editor.

> Changed your model in Blender? Re-export over the same FBX file, then click **Update My Thing**. Your script and your sounds are kept.

---

## 3. Write your Thing's behavior

Open your script. It already contains every event, mostly empty. A Thing script is a list of **"when this happens, do that"**:

```csharp
protected override void OnMeetSomeone(Thing other)   // WHEN I meet someone...
{
    Speak("hello, " + other.ThingName);              // ...say hello
    Spark(Color.yellow, 40);                         // ...and make yellow sparks
}
```

- The lines between `{` and `}` are what happens.
- Each action ends with a semicolon `;`.
- Text goes in quotes: `"hello"`.
- Decimal numbers need an `f`: `1.5f`.
- Colors: `Color.red`, `Color.blue`, `Color.white` … or your own: `new Color(1f, 0.5f, 0f)` (red, green, blue from 0 to 1).
- Lines starting with `//` are notes for humans. The computer ignores them.
- Save the file (Cmd/Ctrl + S), go back to Unity, wait for it to compile, press **Play**.

### A little more C#

**Chance** — do something only sometimes:

```csharp
if (Random.Range(0f, 1f) < 0.3f)   // 30% of the time
{
    Speak("achoo!");
}
```

**Choose** between options:

```csharp
if (IsDay)
{
    Speak("good morning");
}
else
{
    Speak("it's late...");
}
```

**Remember** something — a variable lives at the top of your class, outside any event:

```csharp
public class MayaThing : Thing
{
    int timesGrabbed = 0;           // a number I remember

    protected override void OnGrabbed()
    {
        timesGrabbed = timesGrabbed + 1;
        Speak("that's " + timesGrabbed + " times now");
    }
}
```

**Pick a random line:**

```csharp
string[] lines = { "hm", "oh!", "who's there?" };   // at the top of your class

Speak(lines[Random.Range(0, lines.Length)]);        // inside an event
```

**Do something later, or again and again:**

```csharp
After(2f, Spark);            // spark once, 2 seconds from now
Repeat(10f, PlayRandomSound); // play a sound every 10 seconds, forever
```

> **Don't write your own `Start()`, `Update()` or `Awake()`.** They switch Thing World off for your character. Use `OnBorn()` and `EveryFrame()` instead.

---

## 4. Reference

### Settings — who you are

Change these inside `Setup()`:

| Setting | Default | Meaning |
|---|---|---|
| `settings.walkSpeed` | `1.2f` | Walking speed in meters per second |
| `settings.turnSpeed` | `240f` | Turning speed in degrees per second |
| `settings.autoWander` | `true` | Walk around on your own |
| `settings.wanderInterval` | `6f` | Seconds between new random destinations |
| `settings.wanderRadius` | `8f` | How far a random destination can be (meters) |
| `settings.neighborRadius` | `3f` | Another Thing closer than this counts as "met" |
| `settings.playerNearRadius` | `2f` | The visitor closer than this counts as "near" |
| `settings.canBeGrabbed` | `true` | Can the visitor pick you up? |
| `settings.speechBubbleOffset` | `0.3f` | Extra height of speech bubbles above your head |
| `settings.myColor` | white | Your signature color (used by `Spark()`) |

### Life cycle

| Method | When it runs |
|---|---|
| `Setup()` | Once, before anything else. Change your settings here. |
| `OnBorn()` | Once, when you appear in the world. |
| `EveryFrame()` | About 72–90 times per second. Keep it light — no `Speak` here! |

### Events — what can happen to you

| Event | When |
|---|---|
| `OnMeetSomeone(Thing other)` | Another Thing comes close. `other` is who it is. |
| `OnLeaveSomeone(Thing other)` | A Thing that was close walks away. |
| `OnNeighborSpeaks(Thing speaker, string words)` | A neighbor says something. `words` is what they said. |
| `OnNeighborSparks(Thing who)` | A neighbor makes sparks. |
| `OnArrived()` | You reached the place you were walking to. |
| `OnSunrise()` | The sun comes up. |
| `OnSunset()` | The sun goes down. |
| `OnPlayerNear()` | The visitor comes close. |
| `OnPlayerLeave()` | The visitor walks away. |
| `OnGrabbed()` | The visitor picks you up. |
| `OnReleased(float throwSpeed)` | The visitor lets go. `throwSpeed`: 0 = put down gently, 5+ = thrown hard (meters per second). |
| `OnLanded()` | You hit the ground and stood back up. |

### Actions — what you can do

**Moving**

| Action | What it does |
|---|---|
| `Wander()` | Walk to a random place nearby |
| `WalkTo(other)` | Walk to another Thing (`other` from an event) |
| `WalkTo(new Vector3(x, 0, z))` | Walk to a point in the world |
| `GoHome()` | Walk back to where you were born |
| `FollowPlayer()` | Keep following the visitor |
| `RunFromPlayer()` | Keep running away from the visitor |
| `StopMoving()` | Stand still (also stops following / fleeing) |
| `StopMoving(3f)` | Stand still for 3 seconds |
| `StartMoving()` | Allowed to move again |
| `SetSpeed(2f)` | Change walking speed |
| `LookAt(other.transform.position)` | Turn to face a Thing |
| `LookAtPlayer()` | Turn to face the visitor |

**Expressing**

| Action | What it does |
|---|---|
| `Speak("words")` | Speech bubble for 3 seconds (at most one every 1.5 seconds) |
| `Speak("words", 6f)` | Speech bubble for 6 seconds |
| `Spark()` | Particle burst in your color |
| `Spark(Color.red, 80)` | Particle burst, choose color and amount |
| `PlaySound(0)` | Play your first sound (see below) |
| `PlayRandomSound()` | Play one of your sounds at random |
| `PlayAnimation(1)` | Play the animation in your `Action1` slot (1, 2 or 3) |
| `SetColor(Color.blue)` | Tint your whole body |
| `ResetColor()` | Back to your original colors |
| `SetSize(1.5f)` | Grow (1 = original size, 0.5 = half) |

**Timing**

| Action | What it does |
|---|---|
| `After(2f, Spark)` | Do something once, after 2 seconds |
| `Repeat(5f, Wander)` | Do something every 5 seconds, forever |

### Things you can ask

| Ask | Answer |
|---|---|
| `IsDay` | `true` during the day |
| `TimeOfDay` | 0–24 (13.5 = 1:30 PM) |
| `NeighborCount` | How many Things are near you |
| `Neighbors` | The list of them |
| `IsPlayerNear` | Is the visitor near? |
| `PlayerDistance` | Distance to the visitor in meters |
| `IsGrabbed` | Is the visitor holding you? |
| `IsFalling` | Are you flying through the air? |
| `IsMoving` | Are you walking? |
| `Height` | How tall you are (meters) |
| `ThingName` | Your name. `other.ThingName` is someone else's. |

### Sounds

1. Put short audio files (`.wav`, `.mp3`, `.ogg`; under 10 seconds) into `Assets/Creators/YourName/Sounds`.
2. Click your prefab in `Resources/Things`. In the Inspector, find **My Sounds** on your Thing script and drag your audio files into the list.
3. `PlaySound(0)` plays the first one, `PlaySound(1)` the second, and so on.

Sounds are 3D: the visitor hears them from where you are.

---

## 5. Recipes

Copy, paste, change.

**Shy** — runs away when the visitor comes close, stops when they leave:

```csharp
protected override void OnPlayerNear()  { RunFromPlayer(); Speak("eek"); }
protected override void OnPlayerLeave() { StopMoving(); StartMoving(); }
```

**Clingy** — follows the visitor around:

```csharp
protected override void OnPlayerNear() { FollowPlayer(); Speak("wait for me!"); }
```

**Gossip** — repeats what neighbors say:

```csharp
protected override void OnNeighborSpeaks(Thing speaker, string words)
{
    Speak(speaker.ThingName + " said: " + words, 4f);
}
```

**Night owl** — sleeps during the day, comes alive at night:

```csharp
protected override void OnSunrise() { StopMoving(); SetColor(Color.gray); Speak("zzz"); }
protected override void OnSunset()  { StartMoving(); ResetColor(); Spark(Color.cyan, 60); }
```

**Dramatic** — reacts to how hard it was thrown:

```csharp
protected override void OnReleased(float throwSpeed)
{
    if (throwSpeed > 5f) Speak("HOW DARE YOU");
    else Speak("thank you for the ride");
}
protected override void OnLanded() { Spark(); PlayAnimation(1); }
```

**Crowd-shy** — leaves when it gets too busy:

```csharp
protected override void OnMeetSomeone(Thing other)
{
    if (NeighborCount > 2) { Speak("too many people"); Wander(); }
}
```

**Chain reaction** — sparks spread from Thing to Thing:

```csharp
protected override void OnNeighborSparks(Thing who) { After(0.5f, Spark); }
```

---

## 6. Test it

1. **Thing World → 2. Open My Test Scene.**
2. Press **Play** (▶ at the top). Click inside the **Game** view so it gets your keyboard.
3. The **XR Interaction Simulator** panel shows the controls: move and turn your head, switch to a hand, move it, and use grip to grab your Thing. Walk close to trigger `OnPlayerNear`.
4. Press **Play** again to stop. **Anything you change while playing is not saved.**
5. Watch the **Console** window (Window → General → Console). Red lines are errors; they start with your Thing's name. Double-click one to jump to the line in your code.

The simulator is good for checking *that* things happen. How it *feels* — scale, distance, being looked at by a 1.7 m self-portrait — you'll find out in the headset.

---

## 7. Build your city pieces

Your pieces are part of one city made by 16 people. Plan them with the class first: what is your district, how big, what is next to it.

1. Model in Blender at **real-world scale** (1 Blender meter = 1 Unity meter). Put the origin at the **bottom center** of each piece. **Apply All Transforms.**
2. Export each piece as FBX (same settings as your character) into `Assets/Creators/YourName/Pieces`.
3. In Unity, click the FBX in the Project window → **Thing World → 5. Make City Piece From Selected Model**. This makes a prefab called `YourName_Something`, adds colliders (so people can walk on it and bump into it) and marks it **Static** (faster).
4. Drag your pieces into your test scene to see them next to your Thing. (Your test scene is just for you — the class city is built separately.)

Materials: use **Universal Render Pipeline → Lit** (or **Unlit** for flat color / glowing looks). For glow, give a Lit material an **Emission** color — never a Light.

---

## Rules for a world that runs smoothly

The headset is a phone-sized computer drawing two images 72–90 times per second, for 16 people's work at once. These rules keep it smooth. **Check My Work** tests most of them for you.

| Rule | Why |
|---|---|
| Everything inside `Assets/Creators/YourName` | Only this folder is exported. |
| Names start with `YourName_` | 16 people's files must not clash. |
| Character: ≤ **15,000 triangles**, ≤ **4 materials**, 0.3–3 m tall | 16 characters must fit. |
| All your pieces together: ≤ **40,000 triangles**; each piece ≤ **4 materials** and ≤ **30 m** in any direction | The whole city must fit. |
| Textures ≤ **2048 px** (1024 is better) | Headset memory. |
| Real-world scale, origin at the bottom | Pieces line up, Things walk on them. |
| Only **URP/Lit, URP/Unlit or Shader Graph** materials | Other shaders turn pink or appear in one eye only. |
| **No lights, cameras, skies, post-processing, audio listeners or scripts** in pieces | The world has one sun and one camera. Behavior belongs in your Thing. |
| Floors and stairs: slopes ≤ 45°, steps ≤ 0.4 m | Things can only walk where it's walkable. |
| Don't edit `Assets/ThingWorld`, `Assets/Samples` or anyone else's folder | Everyone shares them. |
| Don't write `Start()`, `Update()` or `Awake()` in your Thing | Use `OnBorn()` and `EveryFrame()`. |
| No `Speak`, `Spark` or sounds inside `EveryFrame()` | 90 times per second is noise, not expression. |

---

## 9. Check and export your work

1. **Thing World → 3. Check My Work.** Red ✖ must be fixed; yellow ⚠ should be. Click **Show** to find the problem, **Fix What I Can** for the automatic fixes (static flags, colliders, oversized textures).
2. **Thing World → 4. Export My Work.** A file called `YourName_ThingWorld_<date>.unitypackage` appears on your Desktop.
3. Hand in that one file. You can export again any time; the newest one counts.

---

## 10. Animation (and motion capture later)

Your character has six animation **slots**:

| Slot | Plays when |
|---|---|
| `Idle` | Standing still |
| `Walk` | Walking (blends with Idle by speed) |
| `Grabbed` | Held by the visitor, or flying through the air |
| `Action1`, `Action2`, `Action3` | You call `PlayAnimation(1)`, `(2)` or `(3)` |

To fill a slot: double-click `Assets/Creators/YourName/Animation/YourName_Animations`. In the Inspector you see the six slots — drag a clip into any of them. Good Mixamo choices: a wave, a jump, a dance, falling or flailing for `Grabbed`. Set every animation FBX to **Humanoid** (Rig tab), and tick **Loop Time** for Idle, Walk and Grabbed.

**Motion capture later:** when you record your own movement, you export it as a Humanoid animation and drop it into one of these slots — nothing else changes. Your Thing will move like you.

---

## 11. When something goes wrong

| Problem | Try |
|---|---|
| Red error in the Console after saving your script | Double-click it. Usually a missing `;`, `}` or `"`, or a typo in an action's name (capital letters matter: `Speak`, not `speak`). |
| "Please don't write your own Update()" | Rename it to `EveryFrame()` (and `Start()` to `OnBorn()`). |
| My Thing doesn't move | Is `settings.autoWander` true? Did you call `StopMoving()` and never `StartMoving()`? |
| My Thing slides instead of walking | Fill the `Walk` slot, and make sure the walk animation was downloaded with **In Place**. |
| My Thing stands in a stiff, strange pose | Fill the `Idle` slot. |
| My Thing is huge, tiny, or under the floor | Apply All Transforms in Blender, re-export, **Update My Thing**. |
| Materials are pink | Change the material's shader to **Universal Render Pipeline/Lit**. |
| Speak does nothing | You can only speak once every 1.5 seconds. Empty text is ignored. |
| Nothing happens when I grab | Is `settings.canBeGrabbed` true? In the simulator, make sure you've switched to a hand, not the head. |
| Check My Work says a file is outside my folder | Move that file (texture, material, sound) into your folder, then check again. |

Still stuck? Copy the **first** red line in the Console and ask.

---

<sub>Thing World by **ShanMu Sun** · © ShanMu Sun · Part of [3D Computer Art](https://github.com/WeitongSun/3D-Computer-Art) · Licensed under [CC BY-NC 4.0](../LICENSE). Inspired by *ThingThingThing* © 2018 zzyw studio, MIT License.</sub>
