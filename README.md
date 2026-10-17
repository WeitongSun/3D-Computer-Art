# Unity + Meta Quest 3 — Setup & Deployment Guide
## Unity + Meta Quest 3 — 安装与部署教程

**Created by [ShanMu Sun](https://shanmu.zip/) · Part of the *3D Computer Art* course.**
**作者：[ShanMu Sun](https://shanmu.zip/) · 《3D Computer Art》课程配套教程。**

> A bilingual (English / 中文), step-by-step guide that takes you from a fresh **Mac or Windows** computer to a working Unity project running on a Meta Quest 3 headset — Unity install, OpenXR + XR Interaction Toolkit, the Meta Quest build profile, headset setup, building, and testing without a headset.
>
> 一份中英文对照的手把手教程，从一台全新的 **Mac 或 Windows** 电脑开始，搭建能部署到 Meta Quest 3 的 Unity 开发环境 —— 涵盖 Unity 安装、OpenXR 与 XR Interaction Toolkit、Meta Quest 构建配置、头显设置、打包，以及没有头显时如何测试。

---

## What's new in version 2 (October 2026) · 第二版更新内容（2026 年 10 月）

- **Unity 6.3 LTS** replaces Unity 2022 LTS. Unity 6.0 LTS support ends in October 2026; 6.3 LTS is supported until December 2027. · 用 **Unity 6.3 LTS** 取代 Unity 2022 LTS。Unity 6.0 LTS 的支持于 2026 年 10 月结束，6.3 LTS 支持到 2027 年 12 月。
- Uses Unity's new **Meta Quest build profile**, which sets most Android/XR settings for you. · 使用 Unity 新的 **Meta Quest 构建配置（Build Profile）**，大部分 Android / XR 设置会自动完成。
- Fixed: Quest 3 controllers use the **Touch Plus** profile (v1 said Touch Pro, which is the Quest Pro controller). · 修正：Quest 3 手柄对应 **Touch Plus** 配置（第一版误写为 Touch Pro，那是 Quest Pro 的手柄）。
- Mac and Windows instructions side by side, including the Windows USB driver. · Mac 与 Windows 步骤并列，包括 Windows 的 USB 驱动。
- New **Part 7: testing without a headset** — essential on Mac, where Quest Link is not available. · 新增**第七部分：没有头显时如何测试** —— Mac 无法使用 Quest Link，这一章尤其重要。
- Simpler package set: OpenXR + XR Interaction Toolkit. The Meta XR All-in-One SDK is now optional. · 简化依赖：OpenXR + XR Interaction Toolkit 即可，Meta XR All-in-One SDK 改为可选。

The original version is preserved unchanged in [archive/v1-2026-04-unity-2022](archive/v1-2026-04-unity-2022/README.md).
第一版原封不动地保存在 [archive/v1-2026-04-unity-2022](archive/v1-2026-04-unity-2022/README.md)。

---

## Versions used in this guide · 本教程使用的版本

| Software · 软件 | Version · 版本 |
|---|---|
| Unity Editor | **Unity 6.3 LTS** (`6000.3.x`) — newest 6000.3 patch shown in Unity Hub · Unity Hub 中最新的 6000.3 补丁版本 |
| Render pipeline · 渲染管线 | Universal Render Pipeline (URP) |
| XR plug-in | Unity **OpenXR Plugin** (`com.unity.xr.openxr`) |
| Interaction · 交互 | **XR Interaction Toolkit 3.x** (`com.unity.xr.interaction.toolkit`) |
| Headset · 头显 | Meta Quest 3 (also works for Quest 3S / Quest 2 · 同样适用于 Quest 3S / Quest 2) |
| Computer · 电脑 | macOS (Apple Silicon or Intel) or Windows 10/11 64-bit |

---

## Table of Contents · 目录

| # | Chapter | 章节 | File |
|---|---------|------|------|
| 1 | Install Unity Hub and Unity 6.3 LTS | 安装 Unity Hub 与 Unity 6.3 LTS | [docs/part-1-unity-install.md](docs/part-1-unity-install.md) |
| 2 | Create a new URP project | 创建 URP 项目 | [docs/part-2-urp-project.md](docs/part-2-urp-project.md) |
| 3 | Meta Quest build profile, OpenXR & XR Interaction Toolkit | Meta Quest 构建配置、OpenXR 与 XR Interaction Toolkit | [docs/part-3-meta-xr-openxr.md](docs/part-3-meta-xr-openxr.md) |
| 4 | Check player & quality settings | 检查 Player 与画质设置 | [docs/part-4-build-settings.md](docs/part-4-build-settings.md) |
| 5 | Headset setup: developer mode, drivers & tools | 头显设置：开发者模式、驱动与工具 | [docs/part-5-sidequest-setup.md](docs/part-5-sidequest-setup.md) |
| 6 | Build & run on the Quest 3 | 打包并在 Quest 3 上运行 | [docs/part-6-build-sideload.md](docs/part-6-build-sideload.md) |
| 7 | Test without a headset (Mac & Windows) | 没有头显时如何测试（Mac 与 Windows） | [docs/part-7-test-without-headset.md](docs/part-7-test-without-headset.md) |
| — | Quick reference & troubleshooting | 快速参考与故障排查 | [docs/quick-reference.md](docs/quick-reference.md) |

> File names are kept from version 1 so existing links still work. · 文件名沿用第一版，以保证已有链接依然有效。

---

## Course projects · 课程项目

- **[Thing World](thing-world/README.md)** — a collective VR "zero-player" world for Quest 3, inspired by *ThingThingThing* (ZZYW). Students build self-portrait creatures and pieces of a shared city; the framework handles the rest. · 一个为 Quest 3 设计的集体创作 VR"零玩家"世界，灵感来自 *ThingThingThing*（ZZYW）。学生制作自画像式的生物与共享城市的局部，其余由框架处理。

---

## How to use this guide · 如何使用本教程

- Students: read each chapter in order. Every chapter links back to this index. · 学生：按顺序阅读每一章；每章底部都有返回目录的链接。
- Instructors: you are welcome to use this guide in your own class. See **How to Cite** below. · 教师：欢迎在你的课堂中使用本教程，请参考下方「如何引用」。
- Teaching on Mac? Read Part 7 before your first class. · 用 Mac 教学？第一节课前请先读第七部分。

---

## How to Cite · 如何引用

If you use this guide — in a class, workshop, blog post, or any other teaching material — you **must** credit the author and link back to this repository. GitHub also shows a **"Cite this repository"** button (from [CITATION.cff](CITATION.cff)).

若你在课程、工作坊、博客或其他教学资料中使用本教程，**必须**署名作者并保留本仓库链接。GitHub 页面右侧的 **"Cite this repository"** 按钮也可以直接生成引用（来自 [CITATION.cff](CITATION.cff)）。

**Attribution text / 署名文本:**

> Adapted from *Unity + Meta Quest 3 Setup & Deployment Guide* (v2) by ShanMu Sun — https://github.com/WeitongSun/3D-Computer-Art
>
> 改编自 ShanMu Sun 所著《Unity + Meta Quest 3 Setup & Deployment Guide》（第二版）—— https://github.com/WeitongSun/3D-Computer-Art

**BibTeX:**

```bibtex
@misc{sun_unity_metaquest3_guide,
  author       = {Sun, ShanMu},
  title        = {Unity + Meta Quest 3 Setup \& Deployment Guide},
  year         = {2026},
  version      = {2.0},
  howpublished = {\url{https://github.com/WeitongSun/3D-Computer-Art}},
  note         = {Part of the 3D Computer Art course}
}
```

---

## License · 许可

This work is licensed under **Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0)**.

本作品采用 **知识共享 署名-非商业性使用 4.0 国际 (CC BY-NC 4.0)** 许可。

You are free to share and adapt the material for non-commercial purposes, **provided that you give appropriate credit to ShanMu Sun and link to this repository**. Commercial use requires explicit permission. See [LICENSE](LICENSE) and [NOTICE](NOTICE.md).

你可以自由分享和改编本材料用于非商业目的，**但必须署名 ShanMu Sun 并附上本仓库链接**。商业用途需另行书面授权。完整条款见 [LICENSE](LICENSE) 与 [NOTICE](NOTICE.md)。

---

<sub>© ShanMu Sun · 3D Computer Art · [github.com/WeitongSun/3D-Computer-Art](https://github.com/WeitongSun/3D-Computer-Art)</sub>
