> **Unity + Meta Quest 3 Setup Guide — Part 6 / 6**
> *Created by [ShanMu Sun](https://github.com/<YOUR-GITHUB-USERNAME>) · 3D Computer Art · [Back to Index](../README.md)*

# Part 6 — Build in Unity & Sideload to Quest 3
# 第六部分 — 构建并侧载到 Quest 3

## Building & Deploying Your App · 构建并部署你的应用

### Step 1 — Build the APK in Unity · 在 Unity 中构建 APK

An **APK** is the Android app package — the file format that runs on Meta Quest.

**APK** 是 Android 应用包，也是 Meta Quest 上运行的文件格式。

- In Unity, go to **File → Build Settings**. Make sure **Android** is selected as the platform. · 打开 **File → Build Settings**，确认平台是 **Android**。
- **Add your scene:** click **Add Open Scenes** to include your current scene in the build. · 点击 **Add Open Scenes** 把当前场景加入构建。
- Click **Build**. Choose a folder to save the APK (create a folder called **Builds** in your project). · 点击 **Build**，选择保存 APK 的文件夹（建议在项目里新建一个 **Builds** 文件夹）。
- Unity will compile and build — this can take 5–15 minutes depending on your project. · Unity 会编译并构建，视项目大小需要 5–15 分钟。
- When done, you will find a `.apk` file in your chosen folder. · 完成后，你选择的文件夹里会出现一个 `.apk` 文件。

---

> **💡 Build vs Build and Run · Build 与 Build and Run 的区别**
> - **Build** creates the APK file only — you then manually install it with SideQuest. · **Build** 只生成 APK，之后用 SideQuest 手动安装。
> - **Build and Run** tries to install directly if a device is connected via ADB. This works but SideQuest gives you more control. · **Build and Run** 在 ADB 已连接时会直接安装，但 SideQuest 给你更多控制。
>
> For class, use **Build** to create the APK, then install via SideQuest. · 课堂上请用 **Build** 生成 APK，再通过 SideQuest 安装。

---

### Step 2 — Sideload the APK with SideQuest · 用 SideQuest 侧载 APK

Make sure your Quest 3 is connected and SideQuest shows a **green dot** before proceeding. · 开始前确认 Quest 3 已连接，且 SideQuest 显示**绿色圆点**。

- In SideQuest, click the **Install APK from folder** button — it looks like a small box with an arrow, top right area of the window. · 在 SideQuest 中点击 **Install APK from folder** 按钮（右上角一个带箭头的小盒子图标）。
- Navigate to your Builds folder and select your `.apk` file. · 导航到你的 Builds 文件夹，选中 `.apk` 文件。
- SideQuest will install it. You will see a progress bar and an **"APK installed successfully"** message. · SideQuest 会安装，期间有进度条，完成后会提示 **"APK installed successfully"**。
- Put on your headset. Go to your **App Library**. · 戴上头显，打开 **App Library**（应用库）。
- **Filter by "Unknown Sources"** (top right of the library). Your app will appear there. · 右上角筛选改为 **"Unknown Sources"**（未知来源），你的应用就在那里。
- Tap it to launch. Your Unity project is now running on the Quest 3! · 点击启动 —— 你的 Unity 项目已经跑在 Quest 3 上了！

---

> **❓ Where is my app? · 找不到应用？**
> Sideloaded apps do **NOT** appear in your main app library. You must filter by **"Unknown Sources"**.
> 侧载的应用**不会**出现在主应用库。必须切到 **"Unknown Sources"**（未知来源）筛选。
>
> This is because they are not verified by Meta. Your own builds will always be in Unknown Sources.
> 因为它们没有经过 Meta 验证。你自己构建的应用永远在 "Unknown Sources"。

---

### Step 3 — Test and Iterate · 测试与迭代

Each time you make changes in Unity and want to test them on the headset, repeat the **Build → Sideload** process. To speed this up: · 每次修改后，要在头显上测试就重复一次 **Build → Sideload**。提速技巧：

- Use **Build and Run** in Unity if your headset is connected via USB — this skips the manual SideQuest step. · 头显已 USB 连接时可用 **Build and Run**，省掉 SideQuest 手动那一步。
- Keep your Builds folder clean — name each APK with a version number (`MyProject_v1.apk`, `MyProject_v2.apk`) so you can go back if needed. · 保持 Builds 文件夹整洁 —— 给每个 APK 加版本号（`MyProject_v1.apk`、`MyProject_v2.apk`），方便回滚。
- Use Unity's **Development Build** checkbox for builds you are testing — this adds the profiler and makes debugging easier. · 测试用的构建勾选 **Development Build** —— 会附带 profiler，调试更方便。

---

### Checklist · 本章检查清单

- [ ] APK successfully built from Unity · 已在 Unity 中成功构建 APK
- [ ] APK installed via SideQuest (success message) · 已通过 SideQuest 安装（有成功提示）
- [ ] App appears under **Unknown Sources** in App Library · 应用出现在 App Library 的 **Unknown Sources** 下
- [ ] App launches and runs on Quest 3 · 应用能在 Quest 3 上启动运行

---

🎉 **Congratulations! You've completed the full pipeline — from empty Unity install to a working build on Quest 3.**
🎉 **恭喜！你已经完成了从零开始到 Quest 3 运行构建的全流程。**

---

**← Previous · 上一章** [Part 5 — Download & Set Up SideQuest](part-5-sidequest-setup.md)
**Reference · 参考 →** [Quick Reference & Useful Links](quick-reference.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/&lt;YOUR-GITHUB-USERNAME&gt;](https://github.com/<YOUR-GITHUB-USERNAME>) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
