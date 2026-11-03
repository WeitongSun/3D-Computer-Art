# Lighting

*1 person · needs the Thing World v1.1 update · [all roles](README.md)*

**Your job:** Light everything under the sky so it matches the sky: sun and moon color, sunset tint, light in the shadows, fog.

## How

1. With Skybox, agree on colors for morning, day, sunset and night (Miro ATMOSPHERE PALETTE).
2. **World Building → Lighting: Create Lighting Settings**. It connects to the Sun of the open scene.
3. Press Play, then change colors on your Lighting asset — you see it live, and changes made during Play are **kept**.
4. Fog: keep it light — thick fog hides the world in VR.
5. Optional lamps: **Lighting: Create Lamp**. For everything else that glows, use an **emissive material** (free).

## Limits

colors and fog: no limit · lamps ≤ 4 for the whole city, no shadows, range < 15 m · no extra sun

## Work with

Skybox — you are the Atmosphere pair. Particles (night effects).

## Checklist

- [ ] Palette agreed with Skybox
- [ ] Lighting v1 exported
- [ ] Final lighting exported

## Common questions

**Which file do I change?**
Your **Lighting** asset in `World/Lighting` — not the Sun object in the scene.

**My changes don't show.**
Press Play. The lighting only runs in Play mode. Changes you make to the Lighting asset during Play are kept.

**Everything is too dark at night.**
Raise **Night Intensity** and the night ambient colors. Nobody can see a pitch-black world in VR.

**Fog hides everything.**
Lower the fog density (0.005–0.015 is usually enough).

**Can I add more than 4 lamps?**
No — lights are expensive in the headset. For anything else that glows, use an emissive material.

**My lamp shows a red error in Check My Work.**
You need the latest update (v1.1 fixed this).

More: [questions for all roles](FAQ.md)

## Every time you hand in

**3. Check My Work** (no red) → **4. Export My Work** → copy the file to the hard drive `01_Hand-in/YourRole`.
