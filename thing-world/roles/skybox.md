# Skybox

*1 person · needs the Thing World v1.1 update · [all roles](README.md)*

**Your job:** Design the sky for every time of day: colors, sun, stars, and an optional painted 360° panorama.

## How

1. With Lighting, agree on colors for morning, day, sunset and night (Miro ATMOSPHERE PALETTE).
2. **World Building → Sky: Create Sky Material**. It becomes the sky of the open scene.
3. Change the Day / Night colors in the Inspector. Press Play to watch a full day.
4. Optional panorama: a 2:1 image (max 4096 × 2048) → drop it into **Panorama**, raise **Panorama strength**. Use alpha to let the gradient show through.

## Limits

panorama ≤ 4096 × 2048

## Work with

Lighting — you are the Atmosphere pair. The sun in your sky IS their light.

## Checklist

- [ ] Palette agreed with Lighting
- [ ] Sky v1 exported
- [ ] Final sky exported

## Common questions

**Where do I see my sky?**
It is applied to the scene you have open. Open your World Test Scene first, then create the sky.

**The sky doesn't change from day to night.**
It only changes in Play mode. Press Play and wait — a day is 2 minutes in the test scene.

**My panorama looks stretched.**
It must be 2:1 (e.g. 4096 × 2048), equirectangular. Square images will stretch.

**My panorama covers the sun and stars.**
Use transparency (alpha) in the image where you want the sky to show through, or lower **Panorama strength**.

**Where is the sun color?**
The sun's color comes from Lighting (DJ). Agree on it together.

More: [questions for all roles](FAQ.md)

## Every time you hand in

**3. Check My Work** (no red) → **4. Export My Work** → copy the file to the hard drive `01_Hand-in/YourRole`.
