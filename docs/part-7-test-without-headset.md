> **Unity + Meta Quest 3 Setup Guide (v2) — Part 7 / 7**
> *Created by [ShanMu Sun](https://shanmu.zip/) · 3D Computer Art · [Back to Index](../README.md)*

# Part 7 — Test Without a Headset (Mac & Windows)
# 第七部分 — 没有头显时如何测试（Mac 与 Windows）

Building to the headset takes minutes. Most of the time you want to press **Play** in Unity and check your idea right away. How you do that depends on your computer.

打包到头显需要好几分钟。大多数时候你只想在 Unity 里按 **Play**，马上看到效果。具体方法取决于你的电脑。

| Method · 方法 | Mac | Windows | Needs headset · 需要头显 |
|---|---|---|---|
| **XR Interaction Simulator** (keyboard + mouse · 键盘 + 鼠标) | ✅ | ✅ | No · 不需要 |
| **Quest Link** (headset plugged in, press Play · 头显连线，按 Play) | ❌ | ✅ | Yes · 需要 |
| **Meta XR Simulator** (Meta's simulator · Meta 的模拟器) | ✅ Apple Silicon | ✅ | No · 不需要 |

> **On a Mac, Quest Link is not available.** Use the XR Interaction Simulator for everyday testing and Build And Run (Part 6) when you need the headset.
> **Mac 无法使用 Quest Link。** 日常测试用 XR Interaction Simulator，需要头显时用 Build And Run（第六部分）。

---

## A. XR Interaction Simulator (recommended · 推荐)

This comes with XR Interaction Toolkit. You imported it as a sample in Part 3.

它是 XR Interaction Toolkit 自带的，第三部分已经作为示例导入。

### Turn it on once · 一次性开启

1. Go to **Edit → Project Settings → XR Plug-in Management → XR Interaction Toolkit**. · 打开 **Edit → Project Settings → XR Plug-in Management → XR Interaction Toolkit**。
2. Check **Use XR Interaction Simulator in scenes**. · 勾选 **Use XR Interaction Simulator in scenes**。
3. Keep **Instantiate In Editor Only** checked. · 保持勾选 **Instantiate In Editor Only**。

Now every time you press **Play**, Unity adds the simulator automatically. It is never added to headset builds, so you don't need to remove anything before building.

之后每次按 **Play**，Unity 都会自动加入模拟器。它不会被打包进头显版本，所以打包前不用删除任何东西。

### Use it · 使用方法

1. Open a scene that has an **XR Origin (XR Rig)** (Part 6, Step 1). · 打开一个含有 **XR Origin (XR Rig)** 的场景（第六部分第 1 步）。
2. Press **Play**, then click inside the **Game** view so it receives your keyboard. · 按 **Play**，然后点击 **Game** 视图，让它接收键盘输入。
3. An on-screen panel shows the controls. You can move and turn the head, switch to a controller, move it, and press grip/trigger to grab things. · 屏幕上的面板会显示操作方式。你可以移动、转动头部，切换到手柄，移动手柄，并按握持/扳机键抓取物体。
4. Press **Play** again to stop. Changes made while playing are **not saved**. · 再按一次 **Play** 停止。运行期间做的修改**不会被保存**。

> **💡 The simulator is good for logic, not for feel · 模拟器适合测逻辑，不适合测体感**
> Use it to check that things happen (an object reacts when grabbed, a sound plays when you get close). Scale, comfort and presence can only be judged in the headset.
> 用它确认"事情有没有发生"（被抓起时有没有反应，靠近时有没有声音）。尺度、舒适度和临场感只能在头显里判断。

---

## B. Quest Link (Windows only · 仅 Windows)

1. Install the **Meta Horizon Link** app on the Windows PC: <https://www.meta.com/help/quest/1517439565442928/> · 在 Windows 电脑上安装 **Meta Horizon Link** 应用。
2. Connect the headset with a USB-C cable (or Air Link over Wi-Fi) and start **Link** from the headset's Quick Settings. · 用 USB-C 线（或通过 Wi-Fi 使用 Air Link）连接头显，并在头显快捷设置中启动 **Link**。
3. In Unity, make sure **OpenXR** is checked on the **desktop** tab of XR Plug-in Management (Part 3, Step 2), and set **Play Mode OpenXR Runtime** to **Oculus / Meta**. · 在 Unity 中确认 XR Plug-in Management 的**桌面**标签已勾选 **OpenXR**（第三部分第 2 步），并将 **Play Mode OpenXR Runtime** 设为 **Oculus / Meta**。
4. Press **Play**. The scene appears in the headset. · 按 **Play**，场景会出现在头显里。
5. Turn off the XR Interaction Simulator while using Link, or the two will fight over the controls. · 使用 Link 时请关闭 XR Interaction Simulator，否则两者会争抢控制。

---

## C. Meta XR Simulator (optional · 可选)

Meta's own simulator runs on Windows and Apple Silicon Macs. It is more realistic but heavier to set up; see Meta's documentation: <https://developers.meta.com/horizon/documentation/unity/xrsim-intro/>

Meta 自家的模拟器可在 Windows 和 Apple Silicon Mac 上运行，更接近真机，但配置更复杂，详见 Meta 文档。

---

### Checklist · 本章检查清单

- [ ] **Use XR Interaction Simulator in scenes** is checked · 已勾选 **Use XR Interaction Simulator in scenes**
- [ ] Pressing Play shows the simulator panel and you can look around · 按 Play 后出现模拟器面板，可以环顾四周
- [ ] You can grab an object with the simulated controller · 能用模拟手柄抓起物体
- [ ] Windows only (optional): Quest Link works with Play · 仅 Windows（可选）：Quest Link 可配合 Play 使用

---

🎉 **Congratulations! You've completed the full pipeline — from a fresh computer to testing in the Editor and running on Quest 3.**
🎉 **恭喜！你已经完成了全流程 —— 从一台全新的电脑，到在编辑器里测试并在 Quest 3 上运行。**

---

**← Previous · 上一章** [Part 6 — Build & Run on the Quest 3](part-6-build-sideload.md)
**Reference · 参考 →** [Quick Reference & Troubleshooting](quick-reference.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/WeitongSun/3D-Computer-Art](https://github.com/WeitongSun/3D-Computer-Art) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
