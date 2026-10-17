> **Unity + Meta Quest 3 Setup Guide (v2) — Part 4 / 7**
> *Created by [ShanMu Sun](https://shanmu.zip/) · 3D Computer Art · [Back to Index](../README.md)*

# Part 4 — Check Player & Quality Settings
# 第四部分 — 检查 Player 与画质设置

The Meta Quest build profile (Part 3) already set the technical settings. In this part you give your app a name and check that nothing is missing.

Meta Quest 构建配置（第三部分）已经完成了技术设置。这一章只需要给应用命名，并检查有没有遗漏。

---

### Step 1 — Name your app · 给应用命名

Go to **File → Build Profiles → Meta Quest → Player Settings** (or **Edit → Project Settings → Player**).

打开 **File → Build Profiles → Meta Quest → Player Settings**（或 **Edit → Project Settings → Player**）。

| Setting · 设置 | Value · 值 |
|---|---|
| Company Name · 公司名 | Your name or school, **no spaces** · 你的名字或学校，**不要空格** |
| Product Name · 产品名 | The name shown in the headset · 头显里显示的名字 |
| Package Name · 包名 (*Other Settings → Identification*) | `com.yourname.projectname`, lowercase, e.g. `com.shanmu.thingworld` · 全小写 |
| Version · 版本 | `0.1`, then increase for each new build if you like · 之后每次打包可以递增 |

> **⚠️ Two apps with the same Package Name overwrite each other on the headset.** In a class sharing headsets, every student needs a unique package name.
> **包名相同的两个应用会在头显上互相覆盖。** 共用头显的课堂里，每个学生的包名必须不同。

---

### Step 2 — Confirm what the build profile set · 确认构建配置已自动设置的项目

In **Player → Other Settings**, these should already be correct. If one is wrong, fix it here.

在 **Player → Other Settings** 中，以下项目应已正确设置；如有不对，在这里修改。

| Setting · 设置 | Expected value · 应为 |
|---|---|
| Graphics APIs · 图形接口 | **Vulkan** |
| Scripting Backend · 脚本后端 | **IL2CPP** |
| Target Architectures · 目标架构 | **ARM64** only · 仅 **ARM64** |
| Minimum API Level · 最低 API | Android 10 (API 29) or higher · 或更高 |
| Target API Level · 目标 API | Android 12L (API 32) or Automatic · 或 Automatic |
| Texture Compression · 纹理压缩 (*File → Build Profiles → Meta Quest*) | **ASTC** |

> **💡 Player Settings Overrides · Player 设置覆盖**
> Most Player Settings are shared with the regular Android platform. To change one only for Meta Quest, open the Meta Quest build profile and enable it under **Player Settings Overrides**.
> 大部分 Player 设置与普通 Android 平台共用。如果只想对 Meta Quest 修改某一项，打开 Meta Quest 构建配置，在 **Player Settings Overrides** 中启用该项。

---

### Step 3 — Quality & performance basics · 画质与性能基础

Quest 3 is a mobile computer that has to draw **two images at 72–90 frames per second**. A scene that runs smoothly on your laptop can stutter in the headset.

Quest 3 是一台移动设备，需要以 **72–90 帧/秒** 同时绘制**两幅画面**。在笔记本上流畅的场景，在头显里可能会卡。

- The build profile adds a **Meta Quest (Build Profile)** quality level. Keep it selected for Meta Quest in **Edit → Project Settings → Quality**. · 构建配置会新增一个 **Meta Quest (Build Profile)** 画质等级，在 **Edit → Project Settings → Quality** 中保持 Meta Quest 使用这一项。
- In your URP asset (usually in `Assets/Settings/`): turn **HDR off**, set **Anti Aliasing (MSAA)** to **4x**, and keep shadows simple. · 在 URP 资源（通常在 `Assets/Settings/`）里：关闭 **HDR**，**抗锯齿（MSAA）**设为 **4x**，阴影尽量简单。
- Avoid heavy post-processing (bloom, depth of field, motion blur). · 避免使用重的后处理（泛光、景深、运动模糊）。
- Keep polygon counts and texture sizes reasonable: textures **2048 px or smaller**, and as few materials per object as possible. · 控制面数和贴图：贴图 **2048 px 或更小**，每个物体的材质越少越好。

---

### Checklist · 本章检查清单

- [ ] Company Name, Product Name and a unique Package Name set · 已设置公司名、产品名和唯一的包名
- [ ] Vulkan, IL2CPP, ARM64, API levels confirmed · 已确认 Vulkan、IL2CPP、ARM64、API 等级
- [ ] Texture Compression = ASTC · 纹理压缩为 ASTC
- [ ] URP asset: HDR off, MSAA 4x · URP 资源：关闭 HDR，MSAA 4x

---

**← Previous · 上一章** [Part 3 — Meta Quest Build Profile, OpenXR & XR Interaction Toolkit](part-3-meta-xr-openxr.md)
**Next · 下一章 →** [Part 5 — Headset Setup](part-5-sidequest-setup.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/WeitongSun/3D-Computer-Art](https://github.com/WeitongSun/3D-Computer-Art) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
