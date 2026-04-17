> **Unity + Meta Quest 3 Setup Guide — Part 3 / 6**
> *Created by [ShanMu Sun](https://github.com/<YOUR-GITHUB-USERNAME>) · 3D Computer Art · [Back to Index](../README.md)*

# Part 3 — Meta XR Packages & OpenXR Setup
# 第三部分 — Meta XR 包与 OpenXR 配置

## Installing Meta XR Packages · 安装 Meta XR 包

To target Meta Quest 3, you need two things: the **XR Plugin Management** system, and the **Meta XR Core SDK**. These give Unity the ability to communicate with the headset, track controllers and hands, and deploy to Android.

要面向 Meta Quest 3 开发，需要两样东西：**XR Plugin Management**（XR 插件管理）和 **Meta XR Core SDK**。它们让 Unity 能与头显通信、跟踪手柄和双手，并部署到 Android。

---

### Step 1 — Install XR Plugin Management · 安装 XR Plugin Management

1. In Unity, go to **Edit → Project Settings**. · 打开 **Edit → Project Settings**（项目设置）。
2. In the left sidebar, scroll down to **XR Plug-in Management**. · 左侧边栏找到 **XR Plug-in Management**。
3. If it is not installed yet, click **Install XR Plugin Management**. Unity will install it automatically. · 若未安装，点击 **Install XR Plugin Management**，Unity 会自动安装。
4. Once installed, you will see tabs at the top of the XR Plug-in Management panel. Click the **Android** tab (the robot icon). · 安装后面板顶部会出现多个平台标签，点击 **Android** 标签（安卓小机器人图标）。
5. Check the box next to **OpenXR**. Unity may ask to restart — allow it. · 勾选 **OpenXR**。若 Unity 提示重启，同意。

---

### Step 2 — Configure OpenXR for Meta Quest · 为 Meta Quest 配置 OpenXR

1. Still in **Project Settings → XR Plug-in Management**, click **OpenXR** in the left sidebar (it appears after enabling it). · 仍在 **Project Settings → XR Plug-in Management**，在左侧点击 **OpenXR**（启用后才会出现）。
2. Make sure you are on the **Android** tab. · 确认处于 **Android** 标签。
3. Under **Interaction Profiles**, click the **+** button and add: · 在 **Interaction Profiles** 下，点击 **+** 添加：
   - **Oculus Touch Controller Profile**
   - **Meta Quest Touch Pro Controller Profile** (for Quest 3 controllers · Quest 3 手柄)
   - **Hand Interaction Profile** (for hand tracking · 手部追踪)
4. Under **OpenXR Feature Groups**, enable: · 在 **OpenXR Feature Groups** 下启用：
   - **Meta Quest: Support** (required · 必选)
   - **Meta Quest: Quest Link** (if you want to test via cable without building · 若想通过数据线实时测试)
5. Click **Project Validation** (also in the XR Plug-in Management section). Fix any red errors by clicking **Fix All**. · 点击 **Project Validation**，点击 **Fix All** 修复所有红色错误。

---

### Step 3 — Install Meta XR All-in-One SDK · 安装 Meta XR All-in-One SDK

The Meta XR All-in-One SDK gives you hand tracking, controller input, scene understanding, passthrough, and all other Meta-specific features.

Meta XR All-in-One SDK 提供手部追踪、手柄输入、场景识别、透视（passthrough）等所有 Meta 专属功能。

Asset Store · 资源商店：open <https://assetstore.unity.com> and search for **Meta XR All-in-One SDK** (打开资源商店搜索 **Meta XR All-in-One SDK**).

1. Click the link above. Sign in with your Unity account. Click **Add to My Assets**. · 点击上方链接，使用 Unity 账号登录，点击 **Add to My Assets**。
2. In Unity, go to **Window → Package Manager**. · 在 Unity 中打开 **Window → Package Manager**。
3. In the top-left dropdown, switch from **Unity Registry** to **My Assets**. · 左上角下拉菜单切换到 **My Assets**。
4. Search for **Meta XR All-in-One SDK**. Select it and click **Download**, then **Import**. · 搜索 **Meta XR All-in-One SDK**，点击 **Download**，再点 **Import**。
5. Accept all the import prompts. Unity may restart — let it. · 一路接受导入提示；Unity 若要重启，放行。
6. After import, go to **Oculus → Tools → Project Setup Tool**. Fix any remaining warnings. · 导入完成后打开 **Oculus → Tools → Project Setup Tool**，修复剩余的警告。

---

> **💡 Also install: XR Interaction Toolkit · 同时安装：XR Interaction Toolkit**
> `Window → Package Manager → Unity Registry → search 'XR Interaction Toolkit' → Install`
> This gives you high-level components like **XR Grab Interactable**, **Ray Interactor**, and **Teleportation**.
> 提供 **XR Grab Interactable**、**Ray Interactor**、**Teleportation** 等高级交互组件。
>
> After installing, also import the **Starter Assets** sample from the XR Interaction Toolkit package page.
> 安装完成后，再从 XR Interaction Toolkit 的包页面导入 **Starter Assets** 示例。

---

### Checklist · 本章检查清单

- [ ] XR Plug-in Management installed · 已安装 XR Plug-in Management
- [ ] OpenXR enabled on Android tab · 已在 Android 标签启用 OpenXR
- [ ] Three interaction profiles added (Oculus Touch, Meta Quest Touch Pro, Hand) · 已添加三个交互配置
- [ ] Meta Quest: Support feature group enabled · 已启用 Meta Quest: Support
- [ ] Project Validation shows no red errors · Project Validation 无红色错误
- [ ] Meta XR All-in-One SDK imported · 已导入 Meta XR All-in-One SDK
- [ ] XR Interaction Toolkit installed (+ Starter Assets imported) · 已安装 XR Interaction Toolkit（并导入 Starter Assets）

---

**← Previous · 上一章** [Part 2 — Create a New URP Project](part-2-urp-project.md)
**Next · 下一章 →** [Part 4 — Build Settings for Quest 3](part-4-build-settings.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/&lt;YOUR-GITHUB-USERNAME&gt;](https://github.com/<YOUR-GITHUB-USERNAME>) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
