> **Unity + Meta Quest 3 Setup Guide (v2) — Part 1 / 7**
> *Created by [ShanMu Sun](https://shanmu.zip/) · 3D Computer Art · [Back to Index](../README.md)*

# Part 1 — Unity Hub & Unity 6.3 LTS
# 第一部分 — Unity Hub 与 Unity 6.3 LTS

## Downloading Unity · 下载 Unity

### Step 1 — Download Unity Hub · 下载 Unity Hub

Unity Hub is the app that manages your Unity installations and projects. **Always install Unity through Unity Hub — never from the raw installer.**

Unity Hub 是管理 Unity 版本和项目的应用程序。**必须通过 Unity Hub 安装 Unity，不要使用独立安装包。**

Download Unity Hub · 下载地址：<https://unity.com/download>

- Unity Hub is free. Sign in with a Unity account and activate a **Personal** (free) or **Student** license when asked. · Unity Hub 免费。按提示登录 Unity 账号，并激活 **Personal**（免费）或 **Student**（学生）许可证。
- **Mac:** download the macOS version. Apple Silicon (M1–M4) and Intel Macs are both supported. Drag Unity Hub into **Applications**. · **Mac：** 下载 macOS 版本，Apple Silicon（M1–M4）和 Intel 芯片都支持。把 Unity Hub 拖进 **Applications**（应用程序）。
- **Windows:** download the Windows version and run the installer. · **Windows：** 下载 Windows 版本并运行安装程序。

---

### Step 2 — Install Unity 6.3 LTS · 安装 Unity 6.3 LTS

We use **Unity 6.3 LTS** (Long Term Support). LTS versions get bug fixes for two years without breaking changes; 6.3 LTS is supported until **December 2027**.

我们使用 **Unity 6.3 LTS**（长期支持版本）。LTS 版本会持续两年修复问题而不引入破坏性改动；6.3 LTS 支持到 **2027 年 12 月**。

> **Everyone in a class should install the exact same version** (for example `6000.3.5f1`, not just "6.3"). Projects opened in a different patch version get re-imported and can cause problems when you share files.
> **同一个班的所有人应安装完全相同的版本**（例如 `6000.3.5f1`，而不仅仅是"6.3"）。用不同补丁版本打开项目会触发重新导入，互相传文件时容易出问题。

1. Open Unity Hub and click **Installs** in the left sidebar. · 打开 Unity Hub，点击左侧 **Installs**（安装）。
2. Click **Install Editor** (top right). · 点击右上角 **Install Editor**（安装编辑器）。
3. Under **Official releases**, find **Unity 6.3 (LTS)** — the version number starts with `6000.3`. Click **Install**. If your instructor gave you an exact version, use that one. · 在 **Official releases** 中找到 **Unity 6.3 (LTS)**，版本号以 `6000.3` 开头，点击 **Install**。如果老师指定了具体版本号，请安装那个版本。
4. On the modules screen, make sure to check: · 在模块选择界面，**务必勾选**：
   - **Android Build Support** · Android 构建支持
   - ↳ **OpenJDK** (sub-option) · OpenJDK（子选项）
   - ↳ **Android SDK & NDK Tools** (sub-option) · Android SDK 与 NDK 工具（子选项）
   - **Windows only, optional:** *Microsoft Visual Studio* if you want a code editor. On Mac, use **Visual Studio Code** (free, separate download). · **仅 Windows，可选：** 需要代码编辑器可勾选 *Microsoft Visual Studio*。Mac 用户请另外下载免费的 **Visual Studio Code**。
5. Click **Install** and wait (20–40 minutes; about 15 GB of disk space). · 点击 **Install** 并等待（约 20–40 分钟，需要约 15 GB 硬盘空间）。

> **💡 Why Android Build Support? · 为什么要 Android 构建支持？**
> Meta Quest runs on Android. To build and deploy to the headset, Unity needs the Android toolchain.
> Meta Quest 底层基于 Android。若要打包部署到头显，Unity 必须安装 Android 工具链。
>
> Forgot it? Go to **Installs → ⚙ (gear icon) next to your Unity version → Add Modules**.
> 忘记勾选？前往 **Installs → 版本旁的 ⚙（齿轮图标）→ Add Modules**（添加模块）补装。

> **⚠️ Mac: "Unity Hub can't be opened" / 无法打开**
> If macOS blocks an app, go to **System Settings → Privacy & Security** and click **Open Anyway**.
> 如果 macOS 阻止打开应用，前往 **系统设置 → 隐私与安全性**，点击 **仍要打开**。

---

### Checklist · 本章检查清单

- [ ] Unity Hub installed and signed in, license activated · Unity Hub 已安装、已登录、许可证已激活
- [ ] Unity 6.3 LTS (`6000.3.x`) installed via Unity Hub · 已通过 Unity Hub 安装 Unity 6.3 LTS（`6000.3.x`）
- [ ] Android Build Support + OpenJDK + Android SDK & NDK Tools installed · 已安装 Android 构建支持、OpenJDK、Android SDK & NDK
- [ ] Same exact version as your classmates · 与同学使用完全相同的版本

---

**Next · 下一章 →** [Part 2 — Create a New URP Project · 创建 URP 项目](part-2-urp-project.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/WeitongSun/3D-Computer-Art](https://github.com/WeitongSun/3D-Computer-Art) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
