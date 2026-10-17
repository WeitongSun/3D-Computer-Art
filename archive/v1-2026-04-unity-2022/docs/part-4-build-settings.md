> **Unity + Meta Quest 3 Setup Guide — Part 4 / 6**
> *Created by [ShanMu Sun](https://github.com/<YOUR-GITHUB-USERNAME>) · 3D Computer Art · [Back to Index](../README.md)*

# Part 4 — Build Settings for Quest 3
# 第四部分 — Quest 3 构建设置

## Configuring Build Settings · 配置构建设置

Before you can build and deploy to the Quest headset, you need to switch Unity's build target to **Android** and configure a few settings.

在构建并部署到 Quest 头显之前，需要将 Unity 的构建目标切换为 **Android**，并做几项关键设置。

---

### Switch Platform to Android · 切换平台为 Android

- Go to **File → Build Settings**. · 打开 **File → Build Settings**。
- Select **Android** from the platform list. · 在平台列表中选择 **Android**。
- Click **Switch Platform** (bottom left). Unity will reimport assets — this may take a minute. · 点击左下角 **Switch Platform**（切换平台）。Unity 会重新导入资源，需要等几分钟。

---

### Player Settings · 玩家设置

Still in Build Settings, click **Player Settings** (bottom left). Configure these: · 仍在 Build Settings 界面，点击左下角 **Player Settings**（玩家设置），按下表配置：

| Setting · 设置                | Value · 值                                                |
|-------------------------------|-----------------------------------------------------------|
| Company Name · 公司名         | Your name or VCU (anything, no spaces) · 你的名字或学校，**不要空格** |
| Product Name · 产品名         | Your project name · 你的项目名                             |
| Package Name · 包名           | `com.yourname.projectname` (e.g. `com.shanmu.myscene`)    |
| Minimum API Level · 最低 API  | Android 10 / API Level 29                                 |
| Target API Level · 目标 API   | Automatic (highest installed) · 自动（已安装的最高版本）   |
| Scripting Backend · 脚本后端  | **IL2CPP**                                                |
| Target Architecture · 目标架构 | **ARM64 only** (uncheck ARMv7) · **仅 ARM64**（取消勾选 ARMv7） |

---

> **⚠️ IL2CPP + ARM64 is required for Meta Quest · Meta Quest 必须使用 IL2CPP + ARM64**
> Meta Quest does **not** support the Mono scripting backend or ARMv7 architecture.
> Meta Quest **不支持** Mono 脚本后端或 ARMv7 架构。
>
> If you skip this, your build will fail with **"IL2CPP required"** errors.
> 如果忽略这一步，构建会失败并报 **"IL2CPP required"** 错误。

---

### Quality & Graphics Settings · 质量与图形设置

- Go to **Edit → Project Settings → Quality**. Set all quality levels to use your URP Asset. · 打开 **Edit → Project Settings → Quality**，将所有质量等级都设为使用你的 URP Asset。
- Go to **Edit → Project Settings → Graphics**. Set the **Scriptable Render Pipeline Settings** to your URP Asset (usually in `Assets/Settings/`). · 打开 **Edit → Project Settings → Graphics**，将 **Scriptable Render Pipeline Settings** 设为你的 URP Asset（通常位于 `Assets/Settings/`）。
- For Quest 3 performance: **Edit → Project Settings → XR Plug-in Management → OpenXR** → set **Render Mode** to **Multi Pass** or **Single Pass Instanced** (**Single Pass Instanced** is faster). · Quest 3 性能建议：**Edit → Project Settings → XR Plug-in Management → OpenXR** 中将 **Render Mode** 设为 **Multi Pass** 或 **Single Pass Instanced**（**Single Pass Instanced** 更快）。

---

### Checklist · 本章检查清单

- [ ] Platform switched to Android · 平台已切换到 Android
- [ ] Package Name set (e.g. `com.yourname.projectname`) · 包名已设置
- [ ] Minimum API Level ≥ 29 · 最低 API ≥ 29
- [ ] Scripting Backend = IL2CPP · 脚本后端为 IL2CPP
- [ ] Target Architecture = ARM64 only · 目标架构仅 ARM64
- [ ] Quality & Graphics pipeline assets set to URP · 质量与图形的管线资源已设为 URP
- [ ] Render Mode set to Single Pass Instanced (recommended) · Render Mode 设为 Single Pass Instanced

---

**← Previous · 上一章** [Part 3 — Meta XR Packages & OpenXR Setup](part-3-meta-xr-openxr.md)
**Next · 下一章 →** [Part 5 — Download & Set Up SideQuest](part-5-sidequest-setup.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/&lt;YOUR-GITHUB-USERNAME&gt;](https://github.com/<YOUR-GITHUB-USERNAME>) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
