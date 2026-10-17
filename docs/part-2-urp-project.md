> **Unity + Meta Quest 3 Setup Guide (v2) — Part 2 / 7**
> *Created by [ShanMu Sun](https://shanmu.zip/) · 3D Computer Art · [Back to Index](../README.md)*

# Part 2 — Create a New URP Project
# 第二部分 — 创建 URP 项目

## Creating Your Project · 创建项目

Always create a new project using the **Universal 3D** template, which uses the **Universal Render Pipeline (URP)**. URP is the pipeline Meta recommends for Quest: it is fast on mobile hardware and supports single-pass stereo rendering.

新建项目时**必须**选择 **Universal 3D** 模板，它使用 **Universal Render Pipeline (URP)**。URP 是 Meta 推荐的 Quest 渲染管线：在移动芯片上性能好，并支持单通道立体渲染。

---

### Before you start: where to save the project · 开始前：项目放在哪里

> **⚠️ Do not save Unity projects in a cloud-synced folder. · 不要把 Unity 项目放在云同步文件夹里。**
> **Mac:** if *iCloud Drive → Desktop & Documents Folders* is turned on, your Desktop and Documents are synced to iCloud. Unity's thousands of small files will sync slowly and can corrupt the project. The same goes for Google Drive, Dropbox and OneDrive.
> **Mac：** 如果开启了 *iCloud 云盘 → "桌面"与"文稿"文件夹*，桌面和文稿会同步到 iCloud。Unity 项目包含成千上万个小文件，同步很慢，还可能损坏项目。Google Drive、Dropbox、OneDrive 同理。
>
> Make a plain folder instead, for example `~/UnityProjects` (Mac) or `C:\UnityProjects` (Windows).
> 请新建一个普通文件夹，例如 `~/UnityProjects`（Mac）或 `C:\UnityProjects`（Windows）。

---

### Steps · 步骤

1. In Unity Hub, click **Projects → New project**. · 在 Unity Hub 中点击 **Projects → New project**（新建项目）。
2. At the top, make sure the Editor version is **6000.3.x (LTS)**. · 顶部确认编辑器版本为 **6000.3.x (LTS)**。
3. Select the template **Universal 3D**. If it shows a download icon, click **Download template** first. · 选择模板 **Universal 3D**。如果显示下载图标，先点 **Download template**。
   - **Do NOT pick "3D (Built-In Render Pipeline)".** · **不要选 "3D (Built-In Render Pipeline)"。**
4. Name your project (no spaces or special characters is safest, e.g. `MyQuestProject`) and choose the folder you made above. · 给项目命名（最好不含空格和特殊字符，例如 `MyQuestProject`），保存位置选上面新建的文件夹。
5. Uncheck **Connect to Unity Cloud** / **Use Unity Version Control** unless your instructor asks for it. · 除非老师要求，否则取消勾选 **Connect to Unity Cloud** / **Use Unity Version Control**。
6. Click **Create project**. The first launch takes a few minutes. · 点击 **Create project**。首次打开需要几分钟。

---

> **⚠️ Pink materials? · 材质变成粉红色？**
> Pink means the shader is not compatible with URP (usually an asset made for the Built-in pipeline).
> 粉红色说明 shader 与 URP 不兼容（通常是为 Built-in 管线制作的资源）。
>
> **Fix · 解决方法:** `Window → Rendering → Render Pipeline Converter` → choose **Built-in to URP** → check **Material Upgrade** → **Initialize And Convert**.
> 打开 `Window → Rendering → Render Pipeline Converter` → 选择 **Built-in to URP** → 勾选 **Material Upgrade** → 点击 **Initialize And Convert**。

---

### Checklist · 本章检查清单

- [ ] Project saved in a non-synced folder · 项目保存在非云同步文件夹中
- [ ] Project created with the **Universal 3D** template · 使用 **Universal 3D** 模板创建项目
- [ ] Editor version is 6000.3.x LTS · 编辑器版本为 6000.3.x LTS
- [ ] No pink materials in the default scene · 默认场景中没有粉红色材质

---

**← Previous · 上一章** [Part 1 — Unity Hub & Unity 6.3 LTS](part-1-unity-install.md)
**Next · 下一章 →** [Part 3 — Meta Quest Build Profile, OpenXR & XR Interaction Toolkit](part-3-meta-xr-openxr.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/WeitongSun/3D-Computer-Art](https://github.com/WeitongSun/3D-Computer-Art) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
