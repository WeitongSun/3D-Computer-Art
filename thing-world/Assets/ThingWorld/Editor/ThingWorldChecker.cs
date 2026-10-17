using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ThingWorldEditor
{
    /// <summary>
    /// "Thing World → 3. Check My Work"
    /// Checks a student's character and city pieces against the class rules, so that
    /// everything still runs smoothly when 16 people's work goes into one headset.
    /// </summary>
    public static class ThingWorldChecker
    {
        public enum Level { Ok, Warning, Error }

        public class Result
        {
            public Level level;
            public string message;
            public UnityEngine.Object context;
            public Result(Level l, string m, UnityEngine.Object c = null) { level = l; message = m; context = c; }
        }

        // ------------------------------------------------------------------

        public static List<Result> Check(string name)
        {
            var results = new List<Result>();
            ThingWorldConfig cfg = ThingWorldConfig.Load() ?? ScriptableObject.CreateInstance<ThingWorldConfig>();

            string folder = ThingWorldPaths.CreatorFolder(name);
            if (!AssetDatabase.IsValidFolder(folder))
            {
                results.Add(new Result(Level.Error, $"Folder {folder} doesn't exist."));
                return results;
            }

            // ---------- Character ----------
            string[] thingPrefabs = FindPrefabs(ThingWorldPaths.ThingsFolder(name));
            if (thingPrefabs.Length == 0)
                results.Add(new Result(Level.Error, "No Thing yet. Use 'Thing World → 1. Create or Update My Thing'."));

            foreach (string path in thingPrefabs)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                string label = $"Character '{prefab.name}'";

                if (prefab.GetComponent<Thing>() == null)
                    results.Add(new Result(Level.Error, $"{label}: has no Thing script. Run 'Update My Thing' (check the Console for red errors first).", prefab));
                CheckMissingScripts(prefab, label, results);

                int tris = CountTriangles(prefab);
                if (tris > cfg.characterMaxTriangles * 2)
                    results.Add(new Result(Level.Error, $"{label}: {tris:N0} triangles — the limit is {cfg.characterMaxTriangles:N0}. Decimate it in Blender.", prefab));
                else if (tris > cfg.characterMaxTriangles)
                    results.Add(new Result(Level.Warning, $"{label}: {tris:N0} triangles — please stay under {cfg.characterMaxTriangles:N0}.", prefab));
                else
                    results.Add(new Result(Level.Ok, $"{label}: {tris:N0} triangles.", prefab));

                int mats = CountMaterials(prefab);
                if (mats > cfg.characterMaxMaterials)
                    results.Add(new Result(Level.Warning, $"{label}: uses {mats} materials — please use {cfg.characterMaxMaterials} or fewer (combine textures in Blender).", prefab));

                float height = MeasureHeight(prefab);
                if (height < cfg.characterMinHeight || height > cfg.characterMaxHeight)
                    results.Add(new Result(Level.Warning, $"{label}: is {height:0.##} m tall — expected {cfg.characterMinHeight}–{cfg.characterMaxHeight} m. Check your export scale.", prefab));

                CheckForbiddenComponents(prefab, label, results, allowScripts: true);
                CheckShaders(prefab, label, results);
                CheckTextures(prefab, label, cfg.maxTextureSize, results);
                CheckDependencies(path, name, label, results);

                Animator anim = prefab.GetComponentInChildren<Animator>();
                if (anim != null && anim.avatar != null && !anim.avatar.isHuman)
                    results.Add(new Result(Level.Warning, $"{label}: is not Humanoid, so Mixamo / motion capture animations won't play.", prefab));
            }

            // ---------- City pieces ----------
            string[] pieces = FindPrefabs(ThingWorldPaths.PiecesFolder(name));
            if (pieces.Length == 0)
                results.Add(new Result(Level.Warning, "No city pieces yet. Use 'Thing World → 5. Make City Piece From Selected Model'."));

            int totalTris = 0;
            foreach (string path in pieces)
            {
                var piece = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                string label = $"Piece '{piece.name}'";

                if (!piece.name.StartsWith(name + "_", StringComparison.Ordinal))
                    results.Add(new Result(Level.Warning, $"{label}: name should start with '{name}_' so nobody's files clash.", piece));

                CheckMissingScripts(piece, label, results);
                CheckForbiddenComponents(piece, label, results, allowScripts: false);
                CheckShaders(piece, label, results);
                CheckTextures(piece, label, cfg.maxTextureSize, results);
                CheckDependencies(path, name, label, results);

                totalTris += CountTriangles(piece);

                int mats = CountMaterials(piece);
                if (mats > cfg.pieceMaxMaterials)
                    results.Add(new Result(Level.Warning, $"{label}: uses {mats} materials — please use {cfg.pieceMaxMaterials} or fewer.", piece));

                Bounds b = PrefabBounds(piece);
                if (b.size.x > cfg.pieceMaxSize || b.size.y > cfg.pieceMaxSize || b.size.z > cfg.pieceMaxSize)
                    results.Add(new Result(Level.Warning, $"{label}: is {b.size.x:0}×{b.size.y:0}×{b.size.z:0} m — bigger than {cfg.pieceMaxSize} m. Check the scale, or talk to the class about space.", piece));

                if (piece.GetComponentsInChildren<Collider>(true).Length == 0)
                    results.Add(new Result(Level.Warning, $"{label}: has no colliders — people and Things will walk through it. ('Fix What I Can' adds them.)", piece));

                if (!IsStatic(piece))
                    results.Add(new Result(Level.Warning, $"{label}: is not marked Static. ('Fix What I Can' does it.)", piece));
            }
            if (pieces.Length > 0)
            {
                Level l = totalTris > cfg.piecesMaxTrianglesPerStudent * 1.5f ? Level.Error
                        : totalTris > cfg.piecesMaxTrianglesPerStudent ? Level.Warning : Level.Ok;
                results.Add(new Result(l, $"All your pieces together: {totalTris:N0} triangles (budget {cfg.piecesMaxTrianglesPerStudent:N0})."));
            }

            // ---------- Anything else ----------
            foreach (string guid in AssetDatabase.FindAssets("t:Scene", new[] { folder }))
            {
                string p = AssetDatabase.GUIDToAssetPath(guid);
                if (p != ThingWorldPaths.TestScenePath(name))
                    results.Add(new Result(Level.Warning, $"Extra scene '{Path.GetFileName(p)}' — scenes are not used in the city; only your pieces and your Thing are.", AssetDatabase.LoadAssetAtPath<SceneAsset>(p)));
            }

            if (!results.Any(r => r.level != Level.Ok))
                results.Add(new Result(Level.Ok, "Everything looks good!"));
            return results;
        }

        // ------------------------------------------------------------------
        //  individual checks
        // ------------------------------------------------------------------

        static string[] FindPrefabs(string folder)
        {
            if (!AssetDatabase.IsValidFolder(folder)) return new string[0];
            return AssetDatabase.FindAssets("t:Prefab", new[] { folder }).Select(AssetDatabase.GUIDToAssetPath).ToArray();
        }

        static void CheckMissingScripts(GameObject go, string label, List<Result> results)
        {
            foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            {
                if (GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject) > 0)
                {
                    results.Add(new Result(Level.Error, $"{label}: '{t.name}' has a missing script. Remove it in the Inspector.", go));
                    return;
                }
            }
        }

        static void CheckForbiddenComponents(GameObject go, string label, List<Result> results, bool allowScripts)
        {
            if (go.GetComponentsInChildren<Light>(true).Length > 0)
                results.Add(new Result(Level.Error, $"{label}: contains a Light. The city has one sun — delete it (glowing materials are fine).", go));
            if (go.GetComponentsInChildren<Camera>(true).Length > 0)
                results.Add(new Result(Level.Error, $"{label}: contains a Camera. Delete it.", go));
            if (go.GetComponentsInChildren<AudioListener>(true).Length > 0)
                results.Add(new Result(Level.Error, $"{label}: contains an Audio Listener. Delete it.", go));
            if (go.GetComponentsInChildren<UnityEngine.Rendering.Volume>(true).Length > 0)
                results.Add(new Result(Level.Error, $"{label}: contains a post-processing Volume. Delete it.", go));
            if (go.GetComponentsInChildren<ReflectionProbe>(true).Length > 0)
                results.Add(new Result(Level.Warning, $"{label}: contains a Reflection Probe. Please delete it.", go));

            if (!allowScripts)
            {
                foreach (MonoBehaviour mb in go.GetComponentsInChildren<MonoBehaviour>(true))
                {
                    if (mb == null) continue;
                    results.Add(new Result(Level.Error, $"{label}: has a script ({mb.GetType().Name}). City pieces can't have scripts — put behavior in your Thing.", go));
                    break;
                }
            }
        }

        public static bool IsShaderOk(Shader s, out bool unknown)
        {
            unknown = false;
            if (s == null) return false;
            string n = s.name;
            if (n == "Hidden/InternalErrorShader") return false;
            string[] ok = { "Universal Render Pipeline/", "Shader Graphs/", "TextMeshPro/", "Sprites/", "UI/", "Skybox/" };
            if (ok.Any(p => n.StartsWith(p, StringComparison.Ordinal))) return true;
            unknown = true;
            return true;
        }

        static void CheckShaders(GameObject go, string label, List<Result> results)
        {
            foreach (Material m in AllMaterials(go))
            {
                if (m == null)
                {
                    results.Add(new Result(Level.Warning, $"{label}: has an empty material slot.", go));
                    continue;
                }
                if (!IsShaderOk(m.shader, out bool unknown))
                    results.Add(new Result(Level.Error, $"{label}: material '{m.name}' is pink (shader doesn't work in URP). Use URP/Lit or Shader Graph.", m));
                else if (unknown)
                    results.Add(new Result(Level.Warning, $"{label}: material '{m.name}' uses '{m.shader.name}', which may not work in the headset. URP/Lit or URP/Unlit is safest.", m));
                else if (m.shader.name.StartsWith("Universal Render Pipeline/Lit") && m.GetFloat("_Surface") > 0.5f)
                    results.Add(new Result(Level.Ok, $"{label}: '{m.name}' is transparent — fine, but use transparency sparingly.", m));
            }
        }

        static void CheckTextures(GameObject go, string label, int max, List<Result> results)
        {
            var seen = new HashSet<Texture>();
            foreach (Material m in AllMaterials(go))
            {
                if (m == null) continue;
                foreach (int id in m.GetTexturePropertyNameIDs())
                {
                    Texture t = m.GetTexture(id);
                    if (t == null || !seen.Add(t)) continue;
                    if (t.width > max || t.height > max)
                        results.Add(new Result(Level.Warning, $"{label}: texture '{t.name}' is {t.width}×{t.height}. Max is {max}. ('Fix What I Can' shrinks it.)", t));
                }
            }
        }

        static void CheckDependencies(string assetPath, string name, string label, List<Result> results)
        {
            string mine = ThingWorldPaths.CreatorFolder(name);
            foreach (string dep in AssetDatabase.GetDependencies(assetPath, true))
            {
                if (dep == assetPath) continue;
                if (ThingWorldPaths.IsInside(dep, mine)) continue;
                if (ThingWorldPaths.IsInside(dep, ThingWorldPaths.Root)) continue;
                if (dep.StartsWith("Packages/", StringComparison.Ordinal)) continue;
                if (dep.StartsWith("Resources/unity_builtin_extra", StringComparison.Ordinal)) continue;
                if (dep.StartsWith("Library/", StringComparison.Ordinal)) continue;
                if (dep.EndsWith(".cs", StringComparison.Ordinal) && ThingWorldPaths.IsInside(dep, "Assets/Samples")) continue;
                results.Add(new Result(Level.Error,
                    $"{label}: uses '{dep}', which is outside your folder, so it won't be in your export. Move it into {mine}.",
                    AssetDatabase.LoadMainAssetAtPath(dep)));
            }
        }

        // ------------------------------------------------------------------
        //  measurements
        // ------------------------------------------------------------------

        public static int CountTriangles(GameObject go)
        {
            long indices = 0;
            foreach (MeshFilter mf in go.GetComponentsInChildren<MeshFilter>(true))
                indices += IndexCount(mf.sharedMesh);
            foreach (SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                indices += IndexCount(smr.sharedMesh);
            return (int)(indices / 3);
        }

        static long IndexCount(Mesh m)
        {
            if (m == null) return 0;
            long n = 0;
            for (int i = 0; i < m.subMeshCount; i++)
                if (m.GetTopology(i) == MeshTopology.Triangles) n += m.GetIndexCount(i);
            return n;
        }

        static IEnumerable<Material> AllMaterials(GameObject go)
        {
            foreach (Renderer r in go.GetComponentsInChildren<Renderer>(true))
            {
                if (r is ParticleSystemRenderer) continue;
                foreach (Material m in r.sharedMaterials) yield return m;
            }
        }

        static int CountMaterials(GameObject go) => AllMaterials(go).Where(m => m != null).Distinct().Count();

        static Bounds PrefabBounds(GameObject go)
        {
            var rends = go.GetComponentsInChildren<Renderer>(true).Where(r => !(r is ParticleSystemRenderer)).ToArray();
            if (rends.Length == 0) return new Bounds(go.transform.position, Vector3.zero);
            Bounds b = rends[0].bounds;
            foreach (Renderer r in rends) b.Encapsulate(r.bounds);
            return b;
        }

        static float MeasureHeight(GameObject go)
        {
            Bounds b = PrefabBounds(go);
            return b.max.y - go.transform.position.y;
        }

        static bool IsStatic(GameObject go)
        {
            foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            {
                if (t.GetComponent<Renderer>() == null) continue;
                if ((GameObjectUtility.GetStaticEditorFlags(t.gameObject) & StaticEditorFlags.BatchingStatic) == 0) return false;
            }
            return true;
        }

        // ------------------------------------------------------------------
        //  automatic fixes
        // ------------------------------------------------------------------

        public static int FixWhatICan(string name)
        {
            int fixes = 0;
            ThingWorldConfig cfg = ThingWorldConfig.Load() ?? ScriptableObject.CreateInstance<ThingWorldConfig>();

            foreach (string path in FindPrefabs(ThingWorldPaths.PiecesFolder(name)))
            {
                GameObject root = PrefabUtility.LoadPrefabContents(path);
                bool changed = false;
                try
                {
                    foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
                    {
                        GameObject g = t.gameObject;
                        if (g.GetComponent<Renderer>() != null &&
                            (GameObjectUtility.GetStaticEditorFlags(g) & StaticEditorFlags.BatchingStatic) == 0)
                        {
                            GameObjectUtility.SetStaticEditorFlags(g, GameObjectUtility.GetStaticEditorFlags(g) | StaticEditorFlags.BatchingStatic | StaticEditorFlags.ContributeGI | StaticEditorFlags.OccluderStatic | StaticEditorFlags.OccludeeStatic);
                            changed = true;
                        }
                    }
                    if (root.GetComponentsInChildren<Collider>(true).Length == 0)
                    {
                        foreach (MeshFilter mf in root.GetComponentsInChildren<MeshFilter>(true))
                        {
                            if (mf.sharedMesh == null) continue;
                            mf.gameObject.AddComponent<MeshCollider>().sharedMesh = mf.sharedMesh;
                            changed = true;
                        }
                    }
                    if (changed)
                    {
                        PrefabUtility.SaveAsPrefabAsset(root, path);
                        fixes++;
                    }
                }
                finally { PrefabUtility.UnloadPrefabContents(root); }
            }

            // Oversized textures inside the student's folder
            foreach (string guid in AssetDatabase.FindAssets("t:Texture2D", new[] { ThingWorldPaths.CreatorFolder(name) }))
            {
                string p = AssetDatabase.GUIDToAssetPath(guid);
                if (AssetImporter.GetAtPath(p) is TextureImporter ti && ti.maxTextureSize > cfg.maxTextureSize)
                {
                    ti.maxTextureSize = cfg.maxTextureSize;
                    ti.SaveAndReimport();
                    fixes++;
                }
            }

            AssetDatabase.SaveAssets();
            return fixes;
        }
    }

    // ======================================================================
    //  Window
    // ======================================================================

    public class CheckMyWorkWindow : EditorWindow
    {
        string creator;
        List<ThingWorldChecker.Result> results = new List<ThingWorldChecker.Result>();
        Vector2 scroll;

        [MenuItem("Thing World/3. Check My Work", priority = 20)]
        static void Open()
        {
            ThingWorldPaths.WithCreator("Check My Work", name =>
            {
                var w = GetWindow<CheckMyWorkWindow>(false, "Check My Work");
                w.creator = name;
                w.Run();
                w.Show();
            });
        }

        [MenuItem("Thing World/Instructor/Check Every Student", priority = 220)]
        static void CheckEveryone()
        {
            int errors = 0, warnings = 0;
            foreach (string name in ThingWorldPaths.AllCreators())
            {
                foreach (var r in ThingWorldChecker.Check(name))
                {
                    if (r.level == ThingWorldChecker.Level.Error) { errors++; Debug.LogError($"[{name}] {r.message}", r.context); }
                    else if (r.level == ThingWorldChecker.Level.Warning) { warnings++; Debug.LogWarning($"[{name}] {r.message}", r.context); }
                }
            }
            EditorUtility.DisplayDialog("Thing World", $"Checked {ThingWorldPaths.AllCreators().Count} students.\n{errors} errors, {warnings} warnings — see the Console.", "OK");
        }

        public void Run()
        {
            results = ThingWorldChecker.Check(creator);
            Repaint();
        }

        void OnGUI()
        {
            if (string.IsNullOrEmpty(creator)) { Close(); return; }

            EditorGUILayout.LabelField($"Checking: {ThingWorldPaths.CreatorFolder(creator)}", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Check Again")) Run();
                if (GUILayout.Button("Fix What I Can"))
                {
                    int n = ThingWorldChecker.FixWhatICan(creator);
                    Debug.Log($"[Thing World] Fixed {n} item(s).");
                    Run();
                }
            }

            int e = results.Count(r => r.level == ThingWorldChecker.Level.Error);
            int w = results.Count(r => r.level == ThingWorldChecker.Level.Warning);
            EditorGUILayout.HelpBox(
                e > 0 ? $"{e} problem(s) must be fixed before you export." :
                w > 0 ? $"No blocking problems. {w} thing(s) worth improving." :
                        "Ready to export!",
                e > 0 ? MessageType.Error : w > 0 ? MessageType.Warning : MessageType.Info);

            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var r in results.OrderByDescending(r => r.level))
            {
                string icon = r.level == ThingWorldChecker.Level.Error ? "console.erroricon.sml"
                            : r.level == ThingWorldChecker.Level.Warning ? "console.warnicon.sml" : "TestPassed";
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    GUILayout.Label(EditorGUIUtility.IconContent(icon), GUILayout.Width(20));
                    GUILayout.Label(r.message, EditorStyles.wordWrappedLabel);
                    if (r.context != null && GUILayout.Button("Show", GUILayout.Width(50)))
                    {
                        Selection.activeObject = r.context;
                        EditorGUIUtility.PingObject(r.context);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
