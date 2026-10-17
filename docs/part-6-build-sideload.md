> **Unity + Meta Quest 3 Setup Guide (v2) — Part 6 / 7**
> *Created by [ShanMu Sun](https://shanmu.zip/) · 3D Computer Art · [Back to Index](../README.md)*

# Part 6 — Build & Run on the Quest 3
# 第六部分 — 打包并在 Quest 3 上运行

An **APK** is an Android app package — the file format that runs on Meta Quest. Unity builds the APK and can install it on the connected headset in one step.

**APK** 是 Android 应用包，也是 Meta Quest 上运行的文件格式。Unity 可以一步完成打包并安装到已连接的头显上。

---

### Step 1 — Make a scene the player can stand in · 准备一个玩家能站进去的场景

A VR scene needs an **XR Origin** — the player's head and hands. Without it, the app shows a frozen view.

VR 场景必须有一个 **XR Origin** —— 玩家的头和手。没有它，应用里画面会卡住不动。

1. In the **Project** window, search `XR Origin (XR Rig)` (from the Starter Assets you imported in Part 3). · 在 **Project** 窗口搜索 `XR Origin (XR Rig)`（来自第三部分导入的 Starter Assets）。
2. Drag it into your scene. Delete the scene's original **Main Camera** — the XR Origin has its own camera. · 把它拖进场景，并删除场景里原有的 **Main Camera** —— XR Origin 自带相机。
3. Add something to stand on (**GameObject → 3D Object → Plane**) and something to look at. · 添加一个能站的地面（**GameObject → 3D Object → Plane**）和一些可看的物体。
4. Save the scene (**Cmd/Ctrl + S**). · 保存场景（**Cmd/Ctrl + S**）。

---

### Step 2 — Build and Run · 打包并运行

1. Connect and wake the headset (Part 5, Step 4). · 连接并唤醒头显（第五部分第 4 步）。
2. Open **File → Build Profiles** and select **Meta Quest**. · 打开 **File → Build Profiles**，选择 **Meta Quest**。
3. Under **Scene List**, click **Add Open Scenes**. Uncheck any scene you don't want; the **top scene** is the one that opens first. · 在 **Scene List** 中点击 **Add Open Scenes**。取消勾选不需要的场景；**最上面的场景**会最先打开。
4. Under **Run Device**, click **Refresh** and select your Quest. · 在 **Run Device** 中点击 **Refresh** 并选择你的 Quest。
5. Click **Build And Run**. Choose where to save the `.apk` (make a `Builds` folder **outside** `Assets`), and name it with a version, e.g. `MyProject_v1.apk`. · 点击 **Build And Run**。选择 `.apk` 保存位置（在 `Assets` **以外**新建一个 `Builds` 文件夹），文件名带版本号，例如 `MyProject_v1.apk`。
6. The first build is slow (10–20 minutes, longer on older Macs). Later builds are much faster. · 第一次打包很慢（10–20 分钟，旧电脑更久），之后会快很多。
7. When it finishes, the app opens in the headset automatically. · 完成后应用会在头显里自动打开。

---

### Step 3 — Finding your app later · 之后如何找到你的应用

Your app is installed but does **not** appear in the main library. In the headset, open **App Library** and change the filter (top right) to **Unknown Sources**.

应用已安装，但**不会**出现在主应用库里。在头显中打开 **App Library（应用库）**，把右上角的筛选改为 **Unknown Sources（未知来源）**。

Apps you build yourself are always under Unknown Sources because they are not distributed through the Meta store.

自己打包的应用不经过 Meta 商店分发，所以永远在 Unknown Sources 里。

---

### Other ways to install an APK · 安装 APK 的其他方式

If someone sends you an `.apk` file (or you used **Build** instead of **Build And Run**):

如果别人发给你一个 `.apk`（或你用了 **Build** 而不是 **Build And Run**）：

- **MQDH:** *Device Manager* → drag the `.apk` onto the window. · **MQDH：** *Device Manager* → 把 `.apk` 拖进窗口。
- **SideQuest:** click **Install APK from folder** and select the `.apk`. · **SideQuest：** 点击 **Install APK from folder**，选择 `.apk`。
- **Command line · 命令行:** `adb install -r MyProject_v1.apk` (`adb` is inside Unity's Android SDK `platform-tools` folder · `adb` 位于 Unity 的 Android SDK `platform-tools` 文件夹中).

---

### Tips for faster iteration · 加快迭代的小技巧

- **Test in the Editor first** with the XR Interaction Simulator (Part 7). Only build when you need to feel it in the headset. · **先在编辑器里用 XR Interaction Simulator 测试**（第七部分），需要在头显里亲身体验时再打包。
- Check **Development Build** while testing — errors then show up in MQDH's log viewer. · 测试期间勾选 **Development Build**，错误信息会显示在 MQDH 的日志查看器里。
- Build fails? Read the first red error in the **Console**, then check Project Validation (Part 3, Step 5). · 打包失败？先看 **Console** 里第一条红色错误，再检查 Project Validation（第三部分第 5 步）。

---

### Checklist · 本章检查清单

- [ ] Scene contains an **XR Origin (XR Rig)** and no extra Main Camera · 场景中有 **XR Origin (XR Rig)**，没有多余的 Main Camera
- [ ] Scene added to the Scene List · 场景已加入 Scene List
- [ ] **Build And Run** succeeded and the app launched in the headset · **Build And Run** 成功，应用在头显中启动
- [ ] You can find the app again under **Unknown Sources** · 能在 **Unknown Sources** 中再次找到应用

---

**← Previous · 上一章** [Part 5 — Headset Setup](part-5-sidequest-setup.md)
**Next · 下一章 →** [Part 7 — Test Without a Headset](part-7-test-without-headset.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/WeitongSun/3D-Computer-Art](https://github.com/WeitongSun/3D-Computer-Art) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
