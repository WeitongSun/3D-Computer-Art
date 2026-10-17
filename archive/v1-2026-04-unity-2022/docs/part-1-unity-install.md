> **Unity + Meta Quest 3 Setup Guide — Part 1 / 6**
> *Created by [ShanMu Sun](https://github.com/<YOUR-GITHUB-USERNAME>) · 3D Computer Art · [Back to Index](../README.md)*

# Part 1 — Unity Hub & Unity 2022 LTS
# 第一部分 — Unity Hub 与 Unity 2022 LTS

## Downloading Unity · 下载 Unity

### Step 1 — Download Unity Hub · 下载 Unity Hub

Unity Hub is the app that manages your Unity installations and projects. **Always install Unity through Unity Hub — never from the raw installer.**

Unity Hub 是管理 Unity 版本和项目的应用程序。**必须通过 Unity Hub 安装 Unity，不要使用独立安装包。**

Download Unity Hub · 下载地址：<https://unity.com/download>

- Unity Hub is free. · Unity Hub 免费。
- Download the installer for your OS (Windows / macOS / Linux). · 下载对应操作系统的安装包。
- Run the installer and follow the prompts. No license needed just to install. · 运行安装包，按提示操作；安装本身不需要许可证。

---

### Step 2 — Install Unity 2022 LTS · 安装 Unity 2022 LTS

We use **Unity 2022 LTS** (Long Term Support) because it is stable, widely supported, and compatible with all current Meta XR packages. **Do not use Unity 6 or newer versions for this class.**

我们使用 **Unity 2022 LTS**（长期支持版本）。它稳定、兼容性好，并且支持所有当前的 Meta XR 包。**本课程请勿使用 Unity 6 或更新版本。**

Direct link · 官方页面：<https://unity.com/releases/2022-lts>

**Install via Unity Hub (recommended) · 通过 Unity Hub 安装（推荐）：**

1. Open Unity Hub. · 打开 Unity Hub。
2. Click the **Installs** tab on the left sidebar. · 点击左侧边栏的 **Installs**（安装）标签。
3. Click **Install Editor** (top right). · 点击右上角 **Install Editor**（安装编辑器）。
4. Find **Unity 2022.3 LTS** — it will be marked *LTS Recommended*. Click **Install**. · 找到 **Unity 2022.3 LTS**，会标记为 *LTS Recommended*，点击 **Install**。
5. On the modules screen, make sure to check: · 在模块选择界面，**务必勾选**：
   - **Android Build Support** · Android 构建支持
   - **Android SDK & NDK Tools** (sub-option) · Android SDK 与 NDK 工具（子选项）
   - **OpenJDK** (sub-option) · OpenJDK（子选项）
6. Click **Install** and wait for it to finish (15–30 minutes). · 点击 **Install** 并等待完成（约 15–30 分钟）。

> **💡 Why Android Build Support? · 为什么要 Android 构建支持？**
> Meta Quest runs on Android. To build and deploy to the headset, Unity needs the Android toolchain installed.
> Meta Quest 底层基于 Android。若要构建并部署到头显，Unity 必须安装 Android 工具链。
>
> If you forgot to install Android Build Support, go to **Installs → gear icon next to your Unity version → Add Modules**.
> 如果忘记勾选，前往 **Installs → 版本旁的齿轮图标 → Add Modules**（添加模块）补装。

---

### Checklist · 本章检查清单

- [ ] Unity Hub installed · Unity Hub 已安装
- [ ] Unity 2022.3 LTS installed via Unity Hub · 已通过 Unity Hub 安装 Unity 2022.3 LTS
- [ ] Android Build Support + SDK/NDK + OpenJDK checked during install · 安装时勾选了 Android 构建支持、SDK/NDK、OpenJDK

---

**Next · 下一章 →** [Part 2 — Create a New URP Project · 创建 URP 项目](part-2-urp-project.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/&lt;YOUR-GITHUB-USERNAME&gt;](https://github.com/<YOUR-GITHUB-USERNAME>) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
