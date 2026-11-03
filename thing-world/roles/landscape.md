# Landscape terraformer

*2 people · needs the Thing World v1.1 update · [all roles](README.md)*

**Your job:** Build the walkable ground of the city: streets, hills, plazas, bridges. You decide where the paths go.

## How

1. Model the ground in Blender in meters (1 unit = 1 m), city center at the origin (0,0,0).
2. Keep the walkable part about **40 × 40 m**; the whole ground ≤ **60 × 60 m**. Go up (hills, stairs, bridges), not out.
3. Slopes ≤ 45°, steps ≤ 0.4 m — otherwise Things can't walk there.
4. Export FBX into `Assets/Creators/YourName/Pieces`, click it → **World Building → Ground: Make Ground From Selected Model**.
5. **World Building → Rebuild My World Test Scene** → Play. Do the blobs walk everywhere they should?
6. Re-export the FBX with the same name to update; the prefab follows.

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

**I updated the FBX — do I run the menu again?**
No. Re-export with the same file name; the ground prefab updates by itself.

**How big should each of the 5 zones be?**
The walkable island is about 40 × 40 m, so each zone is roughly 15 × 15 m. Draw it on the Miro city map first.

More: [questions for all roles](FAQ.md)

## Every time you hand in

**3. Check My Work** (no red) → **4. Export My Work** → copy the file to the hard drive `01_Hand-in/YourRole`.
