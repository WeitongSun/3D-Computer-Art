<!--
  ============================================================
  Unity + Meta Quest 3 Setup & Deployment Guide
  Created by ShanMu Sun · 3D Computer Art
  GitHub: https://github.com/<YOUR-GITHUB-USERNAME>
  Repo:   https://github.com/<YOUR-GITHUB-USERNAME>/unity-metaquest3-setup-guide

  BEFORE YOU PUSH: Find-and-replace "<YOUR-GITHUB-USERNAME>"
  across every file in this repo with your real GitHub handle.
  ============================================================
-->

# Unity + Meta Quest 3 — Setup & Deployment Guide
## Unity + Meta Quest 3 — 安装与部署教程

**Created by [ShanMu Sun](https://shanmu.zip/) · Part of the *3D Computer Art* course.**
**作者：[ShanMu Sun](https://shanmu.zip/) · 《3D Computer Art》课程配套教程。**

> A bilingual (English / 中文), step-by-step guide that takes you from a fresh computer to a working Unity project running on a Meta Quest 3 headset — Unity install, Meta XR packages, build configuration, and sideloading via SideQuest.
>
> 一份中英文对照的手把手教程，从零开始把一台新电脑配置成能部署到 Meta Quest 3 的 Unity 开发环境 —— 涵盖 Unity 安装、Meta XR 包配置、构建设置、以及通过 SideQuest 侧载。

---

## Table of Contents · 目录

| # | Chapter | 章节 | File |
|---|---------|------|------|
| 1 | Download & Install Unity Hub and Unity 2022 LTS | 下载并安装 Unity Hub 与 Unity 2022 LTS | [docs/part-1-unity-install.md](docs/part-1-unity-install.md) |
| 2 | Create a new URP project for Meta Quest | 为 Meta Quest 创建 URP 项目 | [docs/part-2-urp-project.md](docs/part-2-urp-project.md) |
| 3 | Install Meta XR packages and configure OpenXR | 安装 Meta XR 包并配置 OpenXR | [docs/part-3-meta-xr-openxr.md](docs/part-3-meta-xr-openxr.md) |
| 4 | Build settings for Android / Quest | Android / Quest 构建设置 | [docs/part-4-build-settings.md](docs/part-4-build-settings.md) |
| 5 | Download and set up SideQuest | 下载与设置 SideQuest | [docs/part-5-sidequest-setup.md](docs/part-5-sidequest-setup.md) |
| 6 | Sideload your APK onto the Quest 3 | 将 APK 侧载到 Quest 3 | [docs/part-6-build-sideload.md](docs/part-6-build-sideload.md) |
| — | Quick Reference & Useful Links | 快速参考与实用链接 | [docs/quick-reference.md](docs/quick-reference.md) |

---

## How to use this guide · 如何使用本教程

- Students: read each chapter in order. Every chapter links back to this index.
- 学生：按顺序阅读每一章；每章底部都有返回目录的链接。
- Instructors: this repo is intended for use in the *3D Computer Art* course. If you use it in your own class, see **How to Cite** below.
- 教师：本教程专为《3D Computer Art》课程编写。若在其他课堂使用，请参考下方「如何引用」。

---

## How to Cite · 如何引用

If you use this guide — in a class, workshop, blog post, or any other teaching material — you **must** credit the author and link back to this repository.

若你在课程、工作坊、博客或其他教学资料中使用本教程，**必须**署名作者并保留本仓库链接。

**Attribution text / 署名文本:**

> Adapted from *Unity + Meta Quest 3 Setup & Deployment Guide* by ShanMu Sun — https://github.com/<YOUR-GITHUB-USERNAME>/unity-metaquest3-setup-guide
>
> 改编自 ShanMu Sun 所著《Unity + Meta Quest 3 Setup & Deployment Guide》—— https://github.com/<YOUR-GITHUB-USERNAME>/unity-metaquest3-setup-guide

**BibTeX:**

```bibtex
@misc{sun_unity_metaquest3_guide,
  author       = {Sun, ShanMu},
  title        = {Unity + Meta Quest 3 Setup \& Deployment Guide},
  year         = {2026},
  howpublished = {\url{https://github.com/<YOUR-GITHUB-USERNAME>/unity-metaquest3-setup-guide}},
  note         = {Part of the 3D Computer Art course}
}
```

---

## License · 许可

This work is licensed under **Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0)**.

本作品采用 **知识共享 署名-非商业性使用 4.0 国际 (CC BY-NC 4.0)** 许可。

You are free to share and adapt the material for non-commercial purposes, **provided that you give appropriate credit to ShanMu Sun and link to this repository**. Commercial use requires explicit permission. See [LICENSE](LICENSE) for the full text.

你可以自由分享和改编本材料用于非商业目的，**但必须署名 ShanMu Sun 并附上本仓库链接**。商业用途需另行书面授权。完整条款见 [LICENSE](LICENSE)。

---

<sub>© ShanMu Sun · 3D Computer Art · [github.com/&lt;YOUR-GITHUB-USERNAME&gt;](https://github.com/<YOUR-GITHUB-USERNAME>)</sub>
