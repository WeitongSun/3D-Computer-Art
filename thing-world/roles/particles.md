# Particle master

*1 person · needs the Thing World v1.1 update · [all roles](README.md)*

**Your job:** Make the air of the city alive: dust, fireflies, rain, smoke, magic.

## How

1. List 3 effect ideas: what, where, and when (day / night / sunset).
2. **World Building → Particles: Create Particle Effect**, then design it in the Particle System (shape, color over lifetime, noise, size).
3. **Day Night Particles → Show When**: Always / Only In The Day / Only At Night / At Sunrise And Sunset.
4. Small, slow and few looks best in VR. Test in **Open My World Test Scene**.

## Limits

≤ 500 particles per effect (Max Particles) · ≤ 1,500 for all your effects

## Work with

Lighting, Landscape, Interaction (effects when something happens)

## Checklist

- [ ] 3 ideas posted on Miro
- [ ] First effects exported
- [ ] Final effects exported

## Common questions

**I don't see my particles.**
Drag the particle prefab into your test scene and press Play. If Show When is Only At Night, wait for night.

**My particles are pink squares.**
The material is missing. Use the particle material the tool made, or Shader → Universal Render Pipeline/Particles/Unlit.

**How do I make them look like fireflies / rain / smoke?**
In the Particle System: Shape (where they start), Start Speed, Gravity Modifier (rain), Color over Lifetime (fade), Noise (wobbly fireflies).

**Check My Work says too many particles.**
Lower **Max Particles** in the Main module. Fewer, bigger, slower particles look better in VR anyway.

More: [questions for all roles](FAQ.md)

## Every time you hand in

**3. Check My Work** (no red) → **4. Export My Work** → copy the file to the hard drive `01_Hand-in/YourRole`.
