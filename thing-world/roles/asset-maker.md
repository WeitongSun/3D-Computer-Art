# Asset Maker

*Everyone not in another role · needs the Thing World v1.1 update · [all roles](README.md)*

**Your job:** Fill the city with your own pieces — buildings, objects, creatures' homes, landmarks — in your own style.

## How

1. Put a sticky note with your name and your idea on the Miro CITY MAP, where you want your piece to go.
2. Model in Blender at real scale (1 unit = 1 m), origin at the bottom center, **Apply All Transforms**.
3. Export FBX into `Assets/Creators/YourName/Pieces`, click it → **Thing World → 5. Make City Piece From Selected Model**.
4. Want it to react (light up, make a sound, be picked up)? Click the prefab → **Thing World → 6. Make Selected Object Interactive**. Ask the Interaction coordinator for help.
5. Test it next to your Thing: **World Building → Open My World Test Scene**.
6. The Textures coordinator will visit you — keep your style, but check texture scale and brightness together.

## Limits

2–4 pieces · all pieces together ≤ 40,000 triangles · each piece ≤ 4 materials and ≤ 30 m · textures ≤ 2048 px · URP materials only · no lights, cameras or scripts (glow = emissive material)

## Rules for the city

Real scale · don't block the paths · big landmarks go at the edges · everything inside your own folder

## Work with

Assets Manager (where it goes), Landscape (paths), Textures, Sound (if your piece needs a sound)

## Checklist

- [ ] Your Thing works and is exported
- [ ] Sticky note on the Miro CITY MAP
- [ ] First piece exported
- [ ] All pieces exported

## Common questions

**My model is huge / tiny / tilted in Unity.**
Blender: 1 unit = 1 m, Apply All Transforms (Ctrl+A), origin at the bottom center. FBX export: tick Apply Transform.

**My textures are missing in Unity.**
Export FBX with Path Mode: Copy and the embed button on, or copy the image files into your folder and drag them onto the material.

**Check My Work says my piece is outside my folder.**
Everything (models, textures, materials) must be inside `Assets/Creators/YourName`. Move it there.

**Too many triangles.**
Use Blender's Decimate modifier. All your pieces together ≤ 40,000.

**Where does my piece go in the city?**
Your sticky note on the Miro city map. The Assets Manager and the teacher place it.

More: [questions for all roles](FAQ.md)

## Every time you hand in

**3. Check My Work** (no red) → **4. Export My Work** → copy the file to the hard drive `01_Hand-in/YourRole`.
