# Interaction coordinator

*1–2 people · needs the Thing World v1.1 update · [all roles](README.md)*

**Your job:** Make the objects the visitor can use: things that react when you come near, things to pick up, buttons, doors.

## How

1. Make a prefab of the object (menu **5**), click it → **Thing World → 6. Make Selected Object Interactive**.
2. In the Inspector, tick a **WHEN**: visitor comes near · points + presses grip · picks it up · a Thing comes near.
3. Fill in what to **DO**: show / hide / toggle an object, play a sound, play particles, open a Simple Mover.
4. Need a sound? Post it on the Miro SOUND ORDERS board.
5. Doors: covered next class.

## Limits

small, light objects · no custom scripts

## Work with

Sound (effect files), Particles, Landscape

## Checklist

- [ ] 3 object ideas on Miro
- [ ] First objects exported
- [ ] Final objects exported

## Common questions

**Menu 6 says it's a model file, not a prefab.**
Make it a city piece first (menu 5), then use menu 6 on the prefab.

**Nothing happens when I come near.**
Check **React To Visitor Near** is ticked and the reaction has something filled in. The blue circle in the Scene view shows the distance.

**How do I test picking up in the simulator?**
Press ] for the right hand, point at the object, hold G to grab, move, release G to throw.

**My object floats / doesn't fall.**
Only objects with **Can Be Picked Up** fall. Everything else stays where you put it.

**How do I add a sound?**
Ask the Sound person for the file (Miro SOUND ORDERS), then drag it into the reaction's Sound slot.

**Doors?**
Covered in class — a door needs a hinge so it turns around its edge, not its center.

More: [questions for all roles](FAQ.md)

## Every time you hand in

**3. Check My Work** (no red) → **4. Export My Work** → copy the file to the hard drive `01_Hand-in/YourRole`.
