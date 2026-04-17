# How to publish this repo · 如何上传这个 repo

> This file is a helper for **you (the author)**, not for students. You can delete it after the repo is live, or keep it as private notes.
> 本文件是给**作者你**看的，不是给学生看的。上传好 repo 之后可以删掉，也可以留着自己参考。

---

## ⚠️ Before you push — 1 critical find-and-replace · 推送前必做的一次替换

Every file uses the placeholder `<YOUR-GITHUB-USERNAME>`. Replace it with your real GitHub handle **before** you push.

所有文件里都有占位符 `<YOUR-GITHUB-USERNAME>`。**推送之前**把它替换成你真实的 GitHub 用户名。

**Quick check — files containing the placeholder · 哪些文件有占位符：**

- `README.md`
- `NOTICE.md`
- `LICENSE`
- `docs/part-1-unity-install.md`
- `docs/part-2-urp-project.md`
- `docs/part-3-meta-xr-openxr.md`
- `docs/part-4-build-settings.md`
- `docs/part-5-sidequest-setup.md`
- `docs/part-6-build-sideload.md`
- `docs/quick-reference.md`

**macOS / Linux (in the repo folder) · 在仓库目录下运行：**

```bash
# Replace <YOUR-GITHUB-USERNAME> with your actual GitHub username
GITHUB_USER="ShanMuSun"   # <-- change this to your real handle

# Preview what will change (dry run)
grep -rl "<YOUR-GITHUB-USERNAME>" .

# Do the replacement
find . -type f \( -name "*.md" -o -name "LICENSE" \) \
  -exec sed -i '' "s/<YOUR-GITHUB-USERNAME>/$GITHUB_USER/g" {} +
# On Linux, replace `sed -i ''` with `sed -i`
```

**Windows (PowerShell) · Windows PowerShell：**

```powershell
$githubUser = "ShanMuSun"   # <-- change this
Get-ChildItem -Recurse -Include *.md,LICENSE | ForEach-Object {
    (Get-Content $_.FullName) -replace '<YOUR-GITHUB-USERNAME>', $githubUser |
        Set-Content $_.FullName
}
```

---

## Step 1 — Create the repo on GitHub · 在 GitHub 上创建 repo

1. Go to <https://github.com/new>.
2. Repository name suggestion: `unity-metaquest3-setup-guide` (matches the URLs used in the files).
3. **Public** (so students can access it).
4. **Do NOT** initialize with README / .gitignore / license — we already have those.
5. Click **Create repository**.

---

## Step 2 — Push this folder · 推送本文件夹

In a terminal, from inside this folder (`unity-metaquest3-setup-guide/`):

```bash
git init
git branch -M main
git add .
git commit -m "Initial commit: Unity + Meta Quest 3 setup guide (bilingual EN/ZH)"
git remote add origin https://github.com/<YOUR-GITHUB-USERNAME>/unity-metaquest3-setup-guide.git
git push -u origin main
```

Replace `<YOUR-GITHUB-USERNAME>` with your actual username.

---

## Step 3 — Polish the repo page · 美化仓库主页

On the GitHub repo page, click the **⚙ (gear)** next to "About" (top-right of the file list) and add:

- **Description:** *Bilingual (EN/中文) step-by-step guide: Unity + Meta Quest 3 setup, build, and sideload. For the 3D Computer Art course.*
- **Website:** your personal site if you have one
- **Topics:** `unity` `meta-quest` `meta-quest-3` `xr` `openxr` `vr` `tutorial` `3d-computer-art` `bilingual`

Pin the repo on your profile so students find it immediately.

---

## Step 4 — Share with students · 分享给学生

Give students **one** link — the repo URL:

```
https://github.com/<YOUR-GITHUB-USERNAME>/unity-metaquest3-setup-guide
```

The README handles everything else (TOC, navigation, attribution).

---

## Adding more content later · 以后加更多内容

The structure is designed to grow. Typical additions:

```
unity-metaquest3-setup-guide/
├── README.md
├── LICENSE
├── NOTICE.md
├── docs/                 ← tutorial chapters
│   ├── part-1-unity-install.md
│   ├── ...
│   └── quick-reference.md
├── assignments/          ← (new) per-assignment briefs
│   └── assignment-01-first-vr-scene.md
├── samples/              ← (new) Unity sample projects or scripts
│   └── README.md
├── troubleshooting/      ← (new) collected FAQ from students
│   └── README.md
└── slides/               ← (new) lecture slide PDFs
```

When you add a new top-level folder, also:

1. Link it from the README's Table of Contents.
2. Add the same attribution header/footer pattern to any new markdown files (copy-paste from an existing chapter).
3. Keep one commit per logical change so the history stays readable.

---

<sub>© ShanMu Sun · 3D Computer Art</sub>
