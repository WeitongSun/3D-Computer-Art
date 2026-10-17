> **Unity + Meta Quest 3 Setup Guide (v2) — Part 5 / 7**
> *Created by [ShanMu Sun](https://shanmu.zip/) · 3D Computer Art · [Back to Index](../README.md)*

# Part 5 — Headset Setup: Developer Mode, Drivers & Tools
# 第五部分 — 头显设置：开发者模式、驱动与工具

To install your own apps on a Quest, the headset must be in **Developer Mode**. This is done once per headset.

要在 Quest 上安装自己的应用，头显必须开启**开发者模式**。每台头显只需设置一次。

> **🎓 For classes · 课堂说明**
> Meta requires the account that enables Developer Mode to be **verified** and **18 or older**. In most classes the **instructor enables Developer Mode on the class headsets** once; students then only need Step 4 (connecting the cable and allowing USB debugging).
> Meta 要求开启开发者模式的账号必须**已验证**且**年满 18 岁**。大多数课堂由**老师统一为课堂头显开启开发者模式**，学生只需要做第 4 步（连线并允许 USB 调试）。

---

### What you need · 所需材料

- A Mac or Windows computer with Unity set up (Parts 1–4) · 一台已按第 1–4 部分配置好 Unity 的 Mac 或 Windows 电脑
- A **data-capable** USB-C cable. Charge-only cables won't work. · 一根**支持数据传输**的 USB-C 线，仅充电线无法使用。
- The **Meta Horizon** app on a phone (iOS or Android), paired with the headset · 手机上已与头显配对的 **Meta Horizon** app（iOS 或 Android）
- A **Meta developer account** in a developer team/organization · 一个加入了开发者团队/组织的 **Meta 开发者账号**

---

### Step 1 — Meta developer account · Meta 开发者账号

1. Go to <https://developers.meta.com/horizon/> and sign in with the **same Meta account that is logged into the headset**. · 打开 <https://developers.meta.com/horizon/>，用**头显上登录的同一个 Meta 账号**登录。
2. Create or join a **developer organization (team)** and accept the developer agreement. · 创建或加入一个**开发者组织（团队）**，并同意开发者协议。
3. Check that your account is **verified** (Developer Dashboard → *Verification*). · 确认账号已**验证**（开发者后台 → *Verification*）。

---

### Step 2 — Turn on Developer Mode · 开启开发者模式

1. Open the **Meta Horizon** app on your phone. · 打开手机上的 **Meta Horizon** app。
2. Tap the **headset icon** in the toolbar, then tap your headset. · 点击工具栏中的**头显图标**，再点你的头显。
3. Tap **Headset Settings → Developer Mode** and switch it **on**. · 点击 **Headset Settings → Developer Mode**，打开开关。
4. Restart the headset. · 重启头显。

> **⚠️ Can't find Developer Mode? · 找不到开发者模式？**
> It only appears after the account has joined a developer organization and is verified (Step 1).
> 只有账号加入了开发者组织并完成验证后（第 1 步），这个选项才会出现。

---

### Step 3 — Windows only: install the USB (ADB) driver · 仅 Windows：安装 USB（ADB）驱动

**Mac users skip this step — macOS needs no driver.** · **Mac 用户跳过这一步 —— macOS 不需要驱动。**

1. Download **Oculus ADB Drivers** from <https://developers.meta.com/horizon/downloads/> (search "ADB"). · 从 <https://developers.meta.com/horizon/downloads/> 下载 **Oculus ADB Drivers**（搜索 "ADB"）。
2. Unzip it and open the `usb_driver` folder. · 解压后打开 `usb_driver` 文件夹。
3. Right-click **`android_winusb.inf`** → **Install**. · 右键 **`android_winusb.inf`** → **安装**。

---

### Step 4 — Connect the headset · 连接头显

1. Turn on the headset and plug it into your computer with the USB-C cable. · 打开头显，用 USB-C 线连接电脑。
2. Put the headset on. When **"Allow USB debugging?"** appears, check **Always allow from this computer** and tap **Allow**. · 戴上头显，出现 **"Allow USB debugging?"** 时，勾选 **Always allow from this computer**，然后点 **Allow**。
3. If you don't see the prompt: on the headset open **Quick Settings → Settings → Developer**, turn on **MTP Notification**, then unplug and replug the cable. · 如果没看到提示：在头显中打开 **快捷设置 → 设置 → Developer（开发者）**，开启 **MTP Notification**，然后拔插一次数据线。
4. In Unity, open **File → Build Profiles → Meta Quest**, click **Refresh** next to **Run Device**. Your Quest should appear in the list. · 在 Unity 中打开 **File → Build Profiles → Meta Quest**，点击 **Run Device** 旁的 **Refresh**，列表中应出现你的 Quest。

---

### Step 5 (optional) — Desktop tools · 第 5 步（可选）— 桌面工具

You can build and install straight from Unity (Part 6), so these are optional. They are useful for installing APK files someone sends you, recording the headset view, or managing apps.

你可以直接从 Unity 打包安装（第六部分），所以以下工具是可选的。它们适合安装别人发给你的 APK、录制头显画面或管理应用。

| Tool · 工具 | Mac | Windows | Notes · 说明 |
|---|---|---|---|
| **Meta Quest Developer Hub (MQDH)** | ✅ | ✅ | Meta's official tool. Install APKs, cast and record, view logs. · Meta 官方工具，可安装 APK、投屏录屏、查看日志。 |
| **SideQuest** | ✅ | ✅ | Free community tool for installing APKs (*Install APK from folder*). · 免费社区工具，用于安装 APK。 |

- MQDH: <https://developers.meta.com/horizon/downloads/> (search "Meta Quest Developer Hub")
- SideQuest: <https://sidequestvr.com/setup-howto>

---

### Checklist · 本章检查清单

- [ ] Developer account verified and in a developer organization · 开发者账号已验证并加入开发者组织
- [ ] Developer Mode ON, headset restarted · 已开启开发者模式并重启头显
- [ ] Windows only: ADB driver installed · 仅 Windows：已安装 ADB 驱动
- [ ] USB debugging allowed ("Always allow from this computer") · 已允许 USB 调试（"始终允许这台电脑"）
- [ ] Quest appears in Unity's **Run Device** list · Quest 出现在 Unity 的 **Run Device** 列表中

---

**← Previous · 上一章** [Part 4 — Check Player & Quality Settings](part-4-build-settings.md)
**Next · 下一章 →** [Part 6 — Build & Run on the Quest 3](part-6-build-sideload.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/WeitongSun/3D-Computer-Art](https://github.com/WeitongSun/3D-Computer-Art) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
