# Landscape terraformer

*2 people · needs the Thing World v1.1 update · [all roles](README.md)*

**Your job:** Build the walkable ground of the city: streets, hills, plazas, bridges. You decide where the paths go.

## How

1. Both of you work in **one Blender file** for the whole island. Model in meters (1 unit = 1 m), island center at the origin (0,0,0).
2. Keep the walkable part about **40 × 40 m**; the whole ground ≤ **60 × 60 m**. Go up (hills, stairs, bridges), not out.
3. Slopes ≤ 45°, steps ≤ 0.4 m — otherwise Things can't walk there.
4. One person exports the FBX as `Island.fbx` into `Assets/Creators/YourName/Pieces`, clicks it → **World Building → Ground: Make Ground From Selected Model**. Do this menu **only once**.
5. **World Building → Rebuild My World Test Scene** → Play. Do the blobs walk everywhere they should?
6. To update: re-export from Blender to the **same place, same name** (`Island.fbx`) and overwrite. The ground in Unity updates by itself — don't run the menu again.

## Limits

≤ 100,000 triangles · ≤ 60 × 60 m · colliders on (the tool adds them)

## Work with

Assets Manager (where pieces go), Textures (ground materials), Interaction

## Checklist

- [ ] Ground blockout exported — simple shapes, right size, paths visible
- [ ] Paths drawn on the Miro CITY MAP
- [ ] Final ground exported

## Common questions

**My ground is huge / tiny in Unity.**
Blender scale: 1 unit = 1 m. Check the size in Blender's N panel before export, and **Apply All Transforms** (Ctrl+A).

**My ground is tilted or lying on its side.**
Apply All Transforms in Blender, and in the FBX export tick **Apply Transform**.

**How do I walk on it?**
**Rebuild My World Test Scene** → Play. The grey placeholder ground hides automatically once your ground exists.

**The blobs don't walk on some parts.**
Too steep (over 45°) or steps too high (over 0.4 m). Make a ramp, or lower the step.

**I updated the ground — do I run the menu again?**
No! Run **Ground: Make Ground From Selected Model** only **once**. After that, just re-export from Blender to the **same place with the same file name** (e.g. always `Island.fbx`) and overwrite it — the ground prefab in Unity updates by itself. Running the menu again makes a second ground (`_1`), and the city ends up with two grounds on top of each other. Made one by accident? Delete the extra `_1` prefab in `World/Ground`.

**We are two people — how do we share the ground?**
Use **one Blender file** for the whole island. Agree on **one person** who exports, always as `Island.fbx`, into their own `Pieces` folder.

**Can we change the ground later?**
Yes, but big changes (moving paths, changing heights) only in the first weeks — other people place their pieces on your ground. After that, only details. Never rename the file.

**How big should each of the 5 zones be?**
The walkable island is about 40 × 40 m, so each zone is roughly 15 × 15 m. Draw it on the Miro city map first.

More: [questions for all roles](FAQ.md)

## Every time you hand in

**3. Check My Work** (no red) → **4. Export My Work** → copy the file to the hard drive `01_Hand-in/YourRole`.
