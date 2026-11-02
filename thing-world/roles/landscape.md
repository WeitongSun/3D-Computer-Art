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

- [ ] Session 5: blockout — simple shapes, right size, paths visible
- [ ] Paths drawn on the Miro CITY MAP
- [ ] Session 8: final ground

## Every time you hand in

**3. Check My Work** (no red) → **4. Export My Work** → copy the file to the hard drive `01_Hand-in/YourName`.
