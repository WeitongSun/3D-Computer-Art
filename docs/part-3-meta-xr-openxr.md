> **Unity + Meta Quest 3 Setup Guide (v2) — Part 3 / 7**
> *Created by [ShanMu Sun](https://shanmu.zip/) · 3D Computer Art · [Back to Index](../README.md)*

# Part 3 — Meta Quest Build Profile, OpenXR & XR Interaction Toolkit
# 第三部分 — Meta Quest 构建配置、OpenXR 与 XR Interaction Toolkit

To run on Meta Quest 3 you need three things:

要在 Meta Quest 3 上运行，需要三样东西：

1. The **Meta Quest build profile** — tells Unity to build for Quest and applies the right Android settings. · **Meta Quest 构建配置** —— 让 Unity 为 Quest 打包，并自动应用正确的 Android 设置。
2. **OpenXR** — the open standard Unity uses to talk to the headset and controllers. Meta recommends OpenXR for all new projects. · **OpenXR** —— Unity 与头显、手柄通信的开放标准。Meta 建议所有新项目使用 OpenXR。
3. **XR Interaction Toolkit (XRI)** — ready-made components for grabbing, pointing, moving and teleporting. · **XR Interaction Toolkit（XRI）** —— 现成的抓取、指向、移动、传送组件。

---

### Step 1 — Switch to the Meta Quest build profile · 切换到 Meta Quest 构建配置

1. Go to **File → Build Profiles**. · 打开 **File → Build Profiles**。
2. In the **Platforms** list on the left, select **Meta Quest**. · 在左侧 **Platforms** 列表中选择 **Meta Quest**。
   - Don't see it? Click **Add Build Profile**, choose **Meta Quest**, then **Add Build Profile**. If it says the module is missing, go back to Part 1 and add Android Build Support. · 没看到？点击 **Add Build Profile**，选择 **Meta Quest**，再点 **Add Build Profile**。如提示缺少模块，回到第一部分补装 Android 构建支持。
3. Click **Switch Platform** (or **Enable Platform**). Unity re-imports the project — this can take several minutes. · 点击 **Switch Platform**（或 **Enable Platform**）。Unity 会重新导入项目，可能需要几分钟。

This automatically installs the **OpenXR Plugin** and sets: Vulkan graphics, IL2CPP, ARM64, minimum API level Android 10 (API 29), target API level Android 12L (API 32), and single-pass instanced rendering.

这一步会自动安装 **OpenXR Plugin**，并设置：Vulkan 图形接口、IL2CPP、ARM64、最低 API 为 Android 10（API 29）、目标 API 为 Android 12L（API 32），以及单通道实例化渲染。

> **💡 Meta Quest vs Android · Meta Quest 与 Android 平台**
> Older guides (including v1 of this one) tell you to pick **Android**. In Unity 6.1 and newer, **Meta Quest** is its own platform with better defaults. Use it.
> 旧教程（包括本教程第一版）让你选择 **Android**。Unity 6.1 起 **Meta Quest** 成为独立平台，默认设置更合适，请使用它。

---

### Step 2 — Configure XR Plug-in Management · 配置 XR 插件管理

1. Go to **Edit → Project Settings → XR Plug-in Management**. · 打开 **Edit → Project Settings → XR Plug-in Management**。
2. Click the **Meta Quest** tab (or the **Android** tab, with the robot icon) and make sure **OpenXR** is checked. · 点击 **Meta Quest** 标签（或带安卓机器人图标的 **Android** 标签），确认已勾选 **OpenXR**。
3. **Windows + Quest Link only:** also click the **desktop** tab (monitor icon) and check **OpenXR** there, so Quest Link works when you press Play (Part 7). **Mac users leave the desktop tab alone** — the XR Interaction Simulator does not need it. · **仅 Windows + Quest Link：** 再点击**桌面**标签（显示器图标），同样勾选 **OpenXR**，这样按 Play 时 Quest Link 才能工作（见第七部分）。**Mac 用户不要改桌面标签** —— XR Interaction Simulator 不需要它。

---

### Step 3 — Configure OpenXR for Quest 3 · 为 Quest 3 配置 OpenXR

1. In the left sidebar, click **XR Plug-in Management → OpenXR**, on the **Meta Quest / Android** tab. · 在左侧点击 **XR Plug-in Management → OpenXR**，停留在 **Meta Quest / Android** 标签。
2. Under **Enabled Interaction Profiles**, click **+** and add: · 在 **Enabled Interaction Profiles** 下点击 **+** 添加：
   - **Oculus Touch Controller Profile** — works with every Quest controller. · 所有 Quest 手柄通用。
   - **Meta Quest Touch Plus Controller Profile** — the Quest 3 / 3S controllers. · Quest 3 / 3S 的手柄。
   - *(optional · 可选)* **Hand Interaction Profile** — only if you use hand tracking. · 仅在使用手势追踪时添加。
3. Under **OpenXR Feature Groups**, enable **Meta Quest Support**. · 在 **OpenXR Feature Groups** 下启用 **Meta Quest Support**。
4. Set **Render Mode** to **Single Pass Instanced** (may be labeled *Multi-view*). · 将 **Render Mode** 设为 **Single Pass Instanced**（有的版本显示为 *Multi-view*）。
5. **Windows + Quest Link only:** switch to the **desktop** tab and add **Oculus Touch Controller Profile** there too. · **仅 Windows + Quest Link：** 切换到**桌面**标签，同样添加 **Oculus Touch Controller Profile**。

> **⚠️ Correction from v1 · 对第一版的更正**
> v1 listed "Meta Quest Touch **Pro** Controller Profile (for Quest 3 controllers)". Touch Pro is the **Quest Pro** controller. Quest 3 uses **Touch Plus**.
> 第一版写的是 "Meta Quest Touch **Pro** Controller Profile（Quest 3 手柄）"。Touch Pro 是 **Quest Pro** 的手柄，Quest 3 用的是 **Touch Plus**。

---

### Step 4 — Install XR Interaction Toolkit and its samples · 安装 XR Interaction Toolkit 及示例

1. Go to **Window → Package Management → Package Manager**. · 打开 **Window → Package Management → Package Manager**。
2. Select **Unity Registry**, search **XR Interaction Toolkit**, and click **Install** (version 3.x). · 选择 **Unity Registry**，搜索 **XR Interaction Toolkit**，点击 **Install**（3.x 版本）。
   - If Unity asks to enable the new **Input System** backend and restart, click **Yes**. · 如果提示启用新的 **Input System** 并重启，点 **Yes**。
3. With XR Interaction Toolkit selected, open the **Samples** tab and click **Import** next to: · 选中 XR Interaction Toolkit，打开 **Samples** 标签，分别点击以下示例旁的 **Import**：
   - **Starter Assets** — includes the ready-made **XR Origin (XR Rig)** prefab (the player's head and hands). · 包含现成的 **XR Origin (XR Rig)** 预制体（玩家的头和手）。
   - **XR Interaction Simulator** — lets you test in the Editor without a headset (Part 7). · 让你在没有头显时也能在编辑器里测试（见第七部分）。
4. Go to **Edit → Project Settings → Tags and Layers** and name **Layer 31** `Teleport`. The Starter Assets' teleport components expect this. · 打开 **Edit → Project Settings → Tags and Layers**，把 **Layer 31** 命名为 `Teleport`。Starter Assets 的传送组件需要这一层。

---

### Step 5 — Project Validation · 项目验证

1. Go to **Edit → Project Settings → XR Plug-in Management → Project Validation**. · 打开 **Edit → Project Settings → XR Plug-in Management → Project Validation**。
2. Check the **Meta Quest / Android** tab (and the **desktop** tab if you enabled it for Quest Link). Click **Fix All** for any red errors. Yellow warnings can usually be fixed the same way. · 检查 **Meta Quest / Android** 标签（如果为 Quest Link 启用了**桌面**标签，也一并检查）。对红色错误点击 **Fix All**；黄色警告一般也可以同样修复。
3. Repeat until no red errors remain. · 重复直到没有红色错误。

---

> **💡 Optional Meta packages · 可选的 Meta 包**
> You do **not** need these for a basic VR project. Add them only when you need the feature:
> 基础 VR 项目**不需要**以下包，用到相应功能时再装：
> - **Unity OpenXR: Meta** (`com.unity.xr.meta-openxr`, Package Manager → Unity Registry) — passthrough / mixed reality, depth, Meta-specific OpenXR features. · 透视 / 混合现实、深度等 Meta 专属 OpenXR 功能。
> - **Meta XR All-in-One SDK** (Asset Store) — Meta's own toolkit (Interaction SDK, Voice, Avatars…). It overlaps with XR Interaction Toolkit; using both in one beginner project is confusing. · Meta 自家的工具包（Interaction SDK、语音、Avatar 等）。它与 XR Interaction Toolkit 功能重叠，初学者不建议在同一项目里同时使用。

---

### Checklist · 本章检查清单

- [ ] Active build profile is **Meta Quest** · 当前构建配置为 **Meta Quest**
- [ ] OpenXR checked on the Meta Quest/Android tab (desktop tab too, Windows + Link only) · Meta Quest/Android 标签已勾选 OpenXR（仅 Windows + Link 需同时勾选桌面标签）
- [ ] Interaction profiles: Oculus Touch + Meta Quest Touch **Plus** · 交互配置：Oculus Touch + Meta Quest Touch **Plus**
- [ ] **Meta Quest Support** feature group enabled · 已启用 **Meta Quest Support**
- [ ] XR Interaction Toolkit 3.x installed; **Starter Assets** and **XR Interaction Simulator** imported · 已安装 XRI 3.x，并导入 **Starter Assets** 与 **XR Interaction Simulator**
- [ ] Layer 31 named `Teleport` · Layer 31 已命名为 `Teleport`
- [ ] Project Validation shows no red errors · Project Validation 无红色错误

---

**← Previous · 上一章** [Part 2 — Create a New URP Project](part-2-urp-project.md)
**Next · 下一章 →** [Part 4 — Check Player & Quality Settings](part-4-build-settings.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/WeitongSun/3D-Computer-Art](https://github.com/WeitongSun/3D-Computer-Art) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
