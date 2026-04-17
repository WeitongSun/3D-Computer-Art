> **Unity + Meta Quest 3 Setup Guide — Part 2 / 6**
> *Created by [ShanMu Sun](https://github.com/<YOUR-GITHUB-USERNAME>) · 3D Computer Art · [Back to Index](../README.md)*

# Part 2 — Create a New URP Project
# 第二部分 — 创建 URP 项目

## Creating Your Project · 创建项目

Always create a new project using the **Universal Render Pipeline (URP)** template — not the default 3D template. Meta Quest requires URP for best performance and compatibility.

新建项目时**必须**选择 **Universal Render Pipeline (URP)** 模板，而不是默认的 3D 模板。Meta Quest 需要 URP 才能获得最佳性能与兼容性。

---

### Steps · 步骤

1. In Unity Hub, click **New Project**. · 在 Unity Hub 中点击 **New Project**（新建项目）。
2. At the top, make sure the Editor version is set to **2022.3.x LTS**. · 顶部确认编辑器版本为 **2022.3.x LTS**。
3. Select the template: **Universal 3D** (Universal Render Pipeline). · 模板选择 **Universal 3D**（URP 管线）。
   - **Do NOT pick "3D (Built-In Render Pipeline)".** · **不要选择 "3D (Built-In Render Pipeline)"。**
4. Name your project and choose a save location. Click **Create Project**. · 命名项目、选择保存路径，然后点击 **Create Project**。
5. Unity will open and set up the project. This may take a few minutes on first launch. · Unity 会打开并初始化项目，首次启动可能较慢。

---

> **⚠️ Check your pipeline · 检查渲染管线**
> If your materials appear **pink**, your shader is not compatible with URP.
> 如果材质显示为**粉红色**，说明 shader 与 URP 不兼容。
>
> **Fix · 解决方法:** `Edit → Rendering → Materials → Convert Selected Built-in Materials to URP`

---

### Checklist · 本章检查清单

- [ ] Project created with Universal 3D (URP) template · 使用 Universal 3D (URP) 模板创建项目
- [ ] Editor version confirmed as 2022.3.x LTS · 已确认编辑器版本为 2022.3.x LTS
- [ ] No pink materials in the default scene · 默认场景中没有粉红色材质

---

**← Previous · 上一章** [Part 1 — Unity Hub & Unity 2022 LTS](part-1-unity-install.md)
**Next · 下一章 →** [Part 3 — Meta XR Packages & OpenXR Setup](part-3-meta-xr-openxr.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/&lt;YOUR-GITHUB-USERNAME&gt;](https://github.com/<YOUR-GITHUB-USERNAME>) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
