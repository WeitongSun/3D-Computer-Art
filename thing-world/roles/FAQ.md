# Common questions (all roles)

[← all roles](README.md)

**I installed the update but don't see the World Building menu.**
Wait until the spinner in the bottom-right of Unity stops. Still missing? Check the Console for red errors and take a screenshot.

**I clicked a menu and it says "First click your file…".**
Click the file in the **Project** window (it turns blue), then use the menu again. Clicking something in the Scene or Hierarchy doesn't count.

**I can't find the `World/…` folder the page talks about.**
It doesn't exist yet — the tool makes it the first time you use your role's menu. Put your files in the folders you already have (`Pieces` for models, `Sounds` for audio) and the tool puts the result in `World/…`.

**Nothing popped up asking for my name.**
Normal — if your project has only your folder, the tools use it automatically.

**What are the 3 blobs in my test scene?**
Test Things, so you can see how your work looks with characters walking around. They are not exported.

**I changed my work but the World Test Scene didn't update.**
**World Building → Rebuild My World Test Scene.**

**I changed something while Play was on and it disappeared.**
Changes to objects in the scene during Play are undone when you stop. Changes to **assets** in the Project window (materials, your Lighting settings, your Sky material) are kept. Stop Play first, then change scene objects.

**How do I see night without waiting?**
In the World Test Scene a full day is 2 minutes, so night comes in about a minute. Or click the **Sun** in the Hierarchy and change **Start Hour** (e.g. 21) before you press Play.

**I walk through hills and walls in the simulator.**
Normal — in the simulator, WASD moves a flying camera that ignores collisions. In the headset, the joystick walks with a body that collides. Test collisions in the headset.

**Something turned pink.**
The material uses a shader that doesn't work in URP. Select the material → Shader → **Universal Render Pipeline/Lit**.

**Check My Work shows red.**
Click **Show** to find the problem, try **Fix What I Can**, then **Check Again**. Yellow is a warning — you can still export.

**Where do I hand in?**
**4. Export My Work** → copy the file from your Desktop to the class hard drive `01_Hand-in/YourRole`. Export again any time; the newest file counts.

**My piece uses a shared material and it's pink in the city.**
The shared library has to be imported too. Tell the Assets Manager which shared library you used.
