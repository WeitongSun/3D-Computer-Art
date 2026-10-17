# Thing World — Instructor Guide

**A collective VR "zero-player" world for Meta Quest 3.**
Created by [ShanMu Sun](https://shanmu.zip/) for *3D Computer Art*. Inspired by [*ThingThingThing*](https://github.com/ZZYW/ThingThingThing) by ZZYW (MIT License).

Every student brings two things into one shared world:

1. **A Thing** — a self-portrait character (modeled in Blender) with a few lines of behavior written in C#. Once the world starts, nobody controls it: it wanders, meets other Things, reacts to sunrise and sunset, and responds when the visitor comes close or picks it up.
2. **Pieces of the city** — models that the class arranges together into one collaged city (think Cao Fei's *RMB City*).

A visitor walks through the city in a Quest 3 headset. Everything difficult — walking, physics, grabbing and throwing, falling and getting back up, sensing neighbors, speech bubbles, sparks, sound, animation — is handled by the framework. Students only fill in events, the way ZZYW's workshop participants did.

> **Students:** you want the **[Handbook](HANDBOOK.md)**, not this page.

---

## Contents

1. [What's in the box](#1-whats-in-the-box)
2. [Build the starter project (once, before class)](#2-build-the-starter-project-once-before-class)
3. [Hand it out](#3-hand-it-out)
4. [What students do](#4-what-students-do)
5. [Assemble the city](#5-assemble-the-city)
6. [Build for Quest 3](#6-build-for-quest-3)
7. [Budgets and rules](#7-budgets-and-rules)
8. [Motion capture later](#8-motion-capture-later)
9. [Troubleshooting](#9-troubleshooting)

---

## 1. What's in the box

```
thing-world/
├── README.md          ← this page (instructors)
├── HANDBOOK.md        ← for students: every event and action, the rules, the workflow
└── Assets/ThingWorld/
    ├── Runtime/       Thing.cs (base class), manager, day/night, player, speech, sparks, config
    ├── Editor/        menus: Create My Thing, Check My Work, Export, Make City Piece, instructor tools
    │   └── Templates/ ThingTemplate.cs.txt — the script every student starts from
    └── Examples/      ExampleBlob.cs — a simple Thing that also serves as a test neighbor
```

All tools live in the **Thing World** menu in Unity's top menu bar:

| Menu | Who | What it does |
|---|---|---|
| **1. Create or Update My Thing** | student | Makes the student's folder, script, animation slots, prefab and test scene from their FBX |
| **2. Open My Test Scene** | student | A small world with the student's Thing, three test Blobs and the player rig |
| **3. Check My Work** | student | Checks triangles, materials, textures, scale, forbidden components, files outside their folder. **Fix What I Can** fixes the easy ones |
| **4. Export My Work** | student | One `.unitypackage` of their folder, saved to the Desktop |
| **5. Make City Piece From Selected Model** | student | Turns a model into a city piece prefab with colliders and static flags |
| Instructor → **Set Up Project** | you | One-time setup: spark material, shared animator, config, test dummy |
| Instructor → **Create City Scene** | you | The final world scene |
| Instructor → **Bake City NavMesh** | you | Makes the arranged city walkable for Things |
| Instructor → **Check Every Student** | you | Runs Check My Work on every folder, results in the Console |

---

## 2. Build the starter project (once, before class)

You make **one** Unity project, set it up, and give a copy to every student. Everyone must start from the same copy: Unity identifies scripts by IDs stored in `.meta` files, and if students install the framework separately, their exported work won't connect to the class project.

1. **Create the project** following the [setup guide](../README.md), Parts 1–4 and 7: Unity 6.3 LTS, *Universal 3D* template, Meta Quest build profile, OpenXR with **Meta Quest Support**, XR Interaction Toolkit 3.x with **Starter Assets** and **XR Interaction Simulator** imported, *Use XR Interaction Simulator in scenes* turned on.
2. **Install AI Navigation:** Package Manager → Unity Registry → **AI Navigation** → Install.
3. **TextMeshPro resources:** Window → TextMeshPro → **Import TMP Essential Resources**.
4. **Copy the framework:** drag the `Assets/ThingWorld` folder from this repository into your project's `Assets` folder. Wait for Unity to compile. A **Thing World** menu appears.
5. Run **Thing World → Instructor → Set Up Project (run once)**. Read the report and fix anything marked ✖.
6. **Player Settings:** set Company Name and a Package Name such as `com.yourschool.thingworld`.
7. **Smoke test:** make a quick Thing yourself (any rigged FBX — a Mixamo character works) with **Thing World → 1. Create or Update My Thing**, press Play, try grabbing it with the simulator, then **Build And Run** once on a Quest 3. Delete your test folder in `Assets/Creators` afterwards.

> On a Mac, Quest Link isn't available. In-editor testing uses the XR Interaction Simulator; feel and scale are checked with Build And Run.

## 3. Hand it out

1. Close Unity.
2. Make a zip of the project folder **without** these folders (Unity rebuilds them): `Library`, `Temp`, `Logs`, `UserSettings`, `Builds`. The zip stays small (tens of MB).
3. Share the zip (drive, LMS, USB). Students unzip it to a non-synced folder (not iCloud Desktop/Documents), then **Unity Hub → Add → Add project from disk**. The first open takes 5–15 minutes.

**The shared Windows computer:** each student who uses it should have their own copy of the starter project in their own folder. The tools figure out whose project it is from the folder name in `Assets/Creators`; if a project contains several names, they ask.

## 4. What students do

The full workflow is in the [Handbook](HANDBOOK.md). In short:

1. Rig the Blender character with **Mixamo**, download it as FBX, and put it in `Assets/Creators/TheirName/Character`.
2. **Thing World → 1. Create or Update My Thing** → their script opens at `Assets/Creators/TheirName/Scripts/TheirNameThing.cs`.
3. Fill in events (`OnMeetSomeone`, `OnGrabbed`, `OnPlayerNear`, `OnSunset` …) with actions (`Speak`, `Spark`, `WalkTo`, `FollowPlayer`, `PlaySound`, `PlayAnimation` …).
4. Build their **city pieces** in Blender, import them into `Assets/Creators/TheirName/Pieces`, and use **5. Make City Piece From Selected Model**.
5. **3. Check My Work** → **4. Export My Work** → hand in the `.unitypackage`.

Everything a student makes lives inside **their own folder**, and the exporter packs only that folder. That is what lets 16 people work in parallel on their own laptops and come together without conflicts.

## 5. Assemble the city

Use a fresh copy of the **same starter project** as the class "master" project (keep it on the shared drive).

1. **Assets → Import Package → Custom Package…** and import each student's `.unitypackage`.
2. **Thing World → Instructor → Check Every Student** and read the Console. Errors (pink materials, lights inside pieces, files outside their folder) should be fixed by the student and re-exported.
3. **Thing World → Instructor → Create City Scene** (once). It contains the sun with the day/night cycle, a placeholder ground (delete it if the class builds its own ground), the walkable-ground baker, the Thing World Manager and the player rig.
4. **Arrange the city together.** Drag pieces from each `Assets/Creators/NAME/Pieces` folder into the scene. This is a good in-class activity on the projector, with students taking turns. Keep everything a student placed as a child of an empty object named after them, so it's easy to find later.
5. **Thing World → Instructor → Bake City NavMesh.** Blue areas in the Scene view are where Things can walk. Re-bake after moving pieces.
6. Press **Play**. Every student's Thing appears automatically at a random walkable spot near the Thing World Manager (move the manager or raise its *Spawn Radius* to spread them out; or add exact **Spawn Points**).

## 6. Build for Quest 3

Open the city scene → **File → Build Profiles → Meta Quest** → make sure the city scene is the only checked scene → **Build And Run**. Details in the [setup guide, Part 6](../docs/part-6-build-sideload.md).

## 7. Budgets and rules

Quest 3 renders two eyes at 72–90 fps. With 16 students, small budgets add up quickly, so the checker enforces these defaults (change them in `Assets/ThingWorld/Resources/ThingWorldConfig.asset`):

| | Budget | Why |
|---|---|---|
| Character | ≤ 15,000 triangles, ≤ 4 materials, 0.3–3 m tall | 16 characters ≈ 240k triangles |
| City pieces (per student, all pieces together) | ≤ 40,000 triangles, ≤ 4 materials per piece, ≤ 30 m in any direction | 16 students ≈ 640k triangles |
| Textures | ≤ 2048 px (1024 recommended) | memory |
| Shaders | URP/Lit, URP/Unlit, Shader Graph | anything else is pink or renders in one eye |
| Not allowed in pieces | lights, cameras, audio listeners, post-processing volumes, scripts | one sun, one camera, one world |
| Files | everything inside `Assets/Creators/TheirName` | otherwise it's missing from the export |

The student-facing version of these rules, with reasons, is in the [Handbook](HANDBOOK.md#rules-for-a-world-that-runs-smoothly).

## 8. Motion capture later

Every character uses one shared animator with six **slots**: `Idle`, `Walk`, `Grabbed`, `Action1`, `Action2`, `Action3`. Each student has their own **Animation Override Controller** (`Assets/Creators/NAME/Animation/NAME_Animations`) that says which clip fills each slot.

- Now: slots are filled with Mixamo clips (or left empty).
- Later: when students record motion capture, they export it as a **Humanoid** FBX clip and drop it into a slot. No code changes, no new prefab. `PlayAnimation(1)` in their script plays whatever is in `Action1`.

That's why **Create My Thing** offers to set rigged characters to **Humanoid**: humanoid clips can be retargeted between any two humanoid characters, so a mocap take recorded on one body plays on another.

## 9. Troubleshooting

| Problem | Fix |
|---|---|
| No **Thing World** menu | There is a compile error. Open the Console; usually a package is missing (XR Interaction Toolkit or AI Navigation). |
| Things don't move | No walkable ground: in the city, run **Bake City NavMesh**; in a test scene, the ground plane is baked automatically on Play. |
| A student's Thing doesn't appear in the city | Its prefab must be in `Assets/Creators/NAME/Resources/Things` and have the student's Thing script on it. **Check Every Student** reports this. |
| Speech bubbles are invisible | Import TMP Essential Resources (Window → TextMeshPro). |
| Sparks are pink or missing | Run **Set Up Project** again. |
| One student's code throws errors | The framework catches exceptions in student events, logs them with the student's class name, and keeps the world running. |
| "Please don't write your own Update()" in the Console | A student wrote `void Update()` / `Start()` in their Thing. They should use `EveryFrame()` / `OnBorn()` instead. |

---

<sub>Thing World by **ShanMu Sun** · © ShanMu Sun · Part of [3D Computer Art](https://github.com/WeitongSun/3D-Computer-Art) · Licensed under [CC BY-NC 4.0](../LICENSE). Concept and API inspired by *ThingThingThing* © 2018 zzyw studio, MIT License.</sub>
