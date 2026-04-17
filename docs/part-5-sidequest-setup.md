> **Unity + Meta Quest 3 Setup Guide — Part 5 / 6**
> *Created by [ShanMu Sun](https://github.com/<YOUR-GITHUB-USERNAME>) · 3D Computer Art · [Back to Index](../README.md)*

# Part 5 — Download & Set Up SideQuest
# 第五部分 — 下载与设置 SideQuest

## Setting Up SideQuest · 配置 SideQuest

SideQuest is a free desktop app that lets you install APK files (Android app packages) directly onto your Quest headset **without going through the Meta Store**. This is how you'll deploy your Unity builds during development.

SideQuest 是一款免费桌面软件，可以**绕过 Meta Store**把 APK（安卓应用包）直接装到 Quest 头显上。开发阶段我们就用它来部署 Unity 构建。

---

### What you need · 所需材料

- A Windows, macOS, or Linux computer · 一台 Windows / macOS / Linux 电脑
- A USB-C cable (the one that came with your Quest works fine) · 一根 USB-C 数据线（Quest 自带的那根就行）
- The **Meta Horizon app** on your phone (iOS or Android) · 手机上的 **Meta Horizon** app（iOS 或 Android）
- A **Meta developer account** (free to create) · 一个 **Meta 开发者账号**（免费注册）

---

### Step 1 — Create a Meta Developer Account · 注册 Meta 开发者账号

Go to: <https://developer.oculus.com> and sign in with the **same Meta/Facebook account linked to your Quest headset**.

前往 <https://developer.oculus.com>，使用**与 Quest 头显绑定的同一个 Meta/Facebook 账号**登录。

Accept the Developer Agreement when prompted. This is free and requires no programming skills.

系统提示时同意开发者协议。免费，无需编程背景。

---

### Step 2 — Enable Developer Mode on your Quest 3 · 在 Quest 3 上开启开发者模式

Developer Mode (now called **Debug Mode** in newer Meta app versions) allows USB debugging and sideloading.

开发者模式（在较新版本的 Meta app 中叫 **Debug Mode**）可以启用 USB 调试和侧载。

- Open the **Meta Horizon app** on your phone. · 打开手机上的 **Meta Horizon** app。
- Tap **Menu** (bottom right) → **Devices** → select your Quest 3. · 点右下角 **Menu** → **Devices** → 选择你的 Quest 3。
- Tap **Developer Mode** or **Debug Mode** and toggle it **ON**. · 点 **Developer Mode** 或 **Debug Mode**，开关打到 **ON**。
- Put on your headset. You may see a prompt — accept it. · 戴上头显，如有提示，同意。
- **Restart your headset** once after enabling. · 启用后**重启一次**头显。

> **⚠️ Can't find Developer Mode? · 找不到开发者模式？**
> If you do not see **Developer Mode**, make sure you have completed the Meta developer account step first.
> 找不到 **Developer Mode** 选项，说明 Meta 开发者账号还没注册好。
>
> The option only appears after you have joined or created a developer organization at developer.oculus.com.
> 必须先在 developer.oculus.com 加入或创建一个开发者组织，这个选项才会出现。

---

### Step 3 — Download SideQuest on your computer · 下载 SideQuest

Download SideQuest · 下载地址：<https://sidequestvr.com/setup-howto>

- Click **Get SideQuest** → choose the **Easy Installer** for your OS. · 点击 **Get SideQuest** → 选择对应系统的 **Easy Installer**。
- Run the installer. On Windows, you may need to confirm a security prompt — click **More info → Run anyway**. · 运行安装包。Windows 用户可能会遇到安全提示，点 **More info → Run anyway**。
- Launch SideQuest after installation. · 安装完成后启动 SideQuest。

---

### Step 4 — Connect your Quest 3 to SideQuest · 连接 Quest 3 与 SideQuest

- Plug your Quest 3 into your computer with a USB-C cable. · 用 USB-C 线把 Quest 3 连到电脑。
- Put on your headset. A prompt will appear: **"Allow USB debugging?"** — tap **Allow**. · 戴上头显，会弹出 **"Allow USB debugging?"**，选 **Allow**。
- Also allow **"Allow access to data"** if prompted (required for file access). · 如果弹出 **"Allow access to data"**，同样选 **Allow**（需要访问文件）。
- Back on your computer in SideQuest, look at the **top right** — the dot should turn **green**. **Green = connected.** · 回到电脑端 SideQuest，看**右上角**的小圆点：变**绿**即为连接成功。

---

> **🔧 Connection troubleshooting · 连接故障排查**
> - If the dot stays red: try a different USB port or cable, or restart SideQuest. · 小圆点一直是红色：换 USB 口、换线，或重启 SideQuest。
> - **Windows users:** SideQuest installs ADB drivers automatically, but if it fails, download Oculus ADB drivers from developer.oculus.com. · **Windows 用户**：SideQuest 会自动装 ADB 驱动，若失败请从 developer.oculus.com 下载 Oculus ADB 驱动。
> - Make sure your headset is on and unlocked — a sleeping headset will not connect. · 头显必须处于开机并解锁状态，休眠中无法连接。

---

### Checklist · 本章检查清单

- [ ] Meta developer account created · Meta 开发者账号已创建
- [ ] Developer / Debug Mode toggled ON in Meta Horizon app · Meta Horizon app 已开启开发者/调试模式
- [ ] Headset restarted after enabling Developer Mode · 头显已重启
- [ ] SideQuest installed on computer · SideQuest 已安装
- [ ] Headset connects to SideQuest (green dot) · 头显成功连接 SideQuest（绿点）

---

**← Previous · 上一章** [Part 4 — Build Settings for Quest 3](part-4-build-settings.md)
**Next · 下一章 →** [Part 6 — Build in Unity & Sideload to Quest 3](part-6-build-sideload.md)

---

<sub>*3D Computer Art* by **ShanMu Sun** · © ShanMu Sun · [github.com/&lt;YOUR-GITHUB-USERNAME&gt;](https://github.com/<YOUR-GITHUB-USERNAME>) · Licensed under [CC BY-NC 4.0](../LICENSE)</sub>
