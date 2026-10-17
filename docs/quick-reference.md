> **Unity + Meta Quest 3 Setup Guide (v2) — Appendix**
> *Created by [ShanMu Sun](https://shanmu.zip/) · 3D Computer Art · [Back to Index](../README.md)*

# Quick Reference & Troubleshooting
# 快速参考与故障排查

## Quick Reference · 快速参考

| Task · 任务 | Where / What · 位置 / 做什么 |
|---|---|
| Download Unity Hub · 下载 Unity Hub | [unity.com/download](https://unity.com/download) |
| Unity version · Unity 版本 | **Unity 6.3 LTS** (`6000.3.x`) + Android Build Support, OpenJDK, Android SDK & NDK |
| Project template · 项目模板 | **Universal 3D** (URP) |
| Switch to Quest · 切换到 Quest | `File → Build Profiles → Meta Quest → Switch Platform` |
| OpenXR settings · OpenXR 设置 | `Edit → Project Settings → XR Plug-in Management → OpenXR` |
| Quest 3 controller profile · Quest 3 手柄配置 | **Meta Quest Touch Plus Controller Profile** (+ Oculus Touch) |
| Install XR Interaction Toolkit · 安装 XRI | `Window → Package Management → Package Manager → Unity Registry` |
| Player rig · 玩家装置 | `XR Origin (XR Rig)` prefab from **Starter Assets** |
| Test without headset · 无头显测试 | `Project Settings → XR Plug-in Management → XR Interaction Toolkit → Use XR Interaction Simulator in scenes` |
| Fix setup problems · 修复配置问题 | `Project Settings → XR Plug-in Management → Project Validation → Fix All` |
| Developer Mode · 开发者模式 | Meta Horizon app → headset icon → your headset → Headset Settings → Developer Mode |
| Windows USB driver · Windows USB 驱动 | Oculus ADB Drivers → right-click `android_winusb.inf` → Install |
| Build to headset · 打包到头显 | `File → Build Profiles → Meta Quest → Run Device → Build And Run` |
| Find your app · 找到你的应用 | Headset: `App Library → Unknown Sources` |

---

## Troubleshooting · 故障排查

| Problem · 问题 | Try this · 试试这样 |
|---|---|
| Materials are pink · 材质是粉红色 | `Window → Rendering → Render Pipeline Converter → Built-in to URP → Material Upgrade` |
| Quest not in **Run Device** list · **Run Device** 列表里没有 Quest | Wake the headset; accept "Allow USB debugging"; try another (data) cable or port; click **Refresh**. Windows: install the ADB driver. · 唤醒头显；同意"允许 USB 调试"；换一根（数据）线或换接口；点 **Refresh**。Windows：安装 ADB 驱动。 |
| App opens but view is frozen or black · 应用打开后画面卡住或全黑 | The scene has no **XR Origin**, or still has an extra Main Camera. Check **Meta Quest Support** is enabled. · 场景没有 **XR Origin**，或还有多余的 Main Camera。检查是否启用了 **Meta Quest Support**。 |
| Controllers don't appear · 看不到手柄 | Add the **Oculus Touch** and **Touch Plus** interaction profiles (Part 3, Step 3). · 添加 **Oculus Touch** 与 **Touch Plus** 交互配置（第三部分第 3 步）。 |
| Something only renders in one eye · 某个物体只在一只眼睛里显示 | That shader doesn't support single-pass instanced rendering. Use URP shaders (URP/Lit, URP/Unlit) or Shader Graph. · 该 shader 不支持单通道实例化渲染。请使用 URP shader（URP/Lit、URP/Unlit）或 Shader Graph。 |
| Build fails with Android/Gradle errors · 打包时报 Android/Gradle 错误 | Make sure OpenJDK and Android SDK & NDK were installed **through Unity Hub** (Part 1). Restart Unity. · 确认 OpenJDK 与 Android SDK & NDK 是**通过 Unity Hub** 安装的（第一部分），然后重启 Unity。 |
| The app replaced a classmate's app · 应用覆盖了同学的应用 | Give each project a unique **Package Name** (Part 4). · 给每个项目设置唯一的 **包名**（第四部分）。 |
| Simulator doesn't respond to keys · 模拟器不响应按键 | Click inside the **Game** view first. · 先点击一下 **Game** 视图。 |
| Project is slow / corrupted on Mac · Mac 上项目很慢或损坏 | Move it out of iCloud-synced Desktop/Documents (Part 2). · 把项目移出 iCloud 同步的桌面/文稿（第二部分）。 |

---

## Useful Links · 实用链接

- **Unity 6 releases & LTS support:** <https://unity.com/releases/unity-6-releases>
- **Meta Quest build profile (Unity 6.3 manual):** <https://docs.unity3d.com/6000.3/Documentation/Manual/xr-meta-quest-build-profile.html>
- **Meta — Unity XR plug-in (OpenXR recommended):** <https://developers.meta.com/horizon/documentation/unity/unity-xr-plugin/>
- **Meta — Set up your device (developer mode, ADB, MQDH):** <https://developers.meta.com/horizon/documentation/unity/unity-env-device-setup/>
- **OpenXR Meta Quest support (Unity):** <https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.17/manual/features/metaquest.html>
- **XR Interaction Toolkit manual:** <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.5/manual/index.html>
- **XR Interaction Simulator settings:** <https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.5/manual/xri-settings.html>
- **Meta XR Simulator:** <https://developers.meta.com/horizon/documentation/unity/xrsim-intro/>
- **SideQuest:** <https://sidequestvr.com/setup-howto>

---

**← Back** [Part 7 — Test Without a Headset](part-7-test-without-headset.md) · [Index · 目录](../README.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/WeitongSun/3D-Computer-Art](https://github.com/WeitongSun/3D-Computer-Art) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
