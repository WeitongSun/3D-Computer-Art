using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ThingWorldEditor
{
    /// <summary>
    /// Tools for the World Building coordinators. Everything a coordinator makes lives in
    ///   Assets/Creators/NAME/World/Ground | Sky | Lighting | Particles | Sounds | Interactive
    /// so "4. Export My Work" still exports one folder, and "3. Check My Work" checks it.
    /// Shared materials live in Assets/Shared/NAME and are exported separately.
    /// </summary>
    public static class WorldBuildingTools
    {
        const string Menu = "Thing World/World Building/";

        // ==================================================================
        //  Everyone: interactive objects
        // ==================================================================

        [MenuItem("Thing World/6. Make Selected Object Interactive", priority = 41)]
        static void MakeInteractive()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                EditorUtility.DisplayDialog("Thing World",
                    "First click an object — in the Hierarchy, or a prefab in the Project window — then use this menu.", "OK");
                return;
            }

            string assetPath = AssetDatabase.GetAssetPath(go);
            if (!string.IsNullOrEmpty(assetPath))
            {
                if (!assetPath.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
                {
                    EditorUtility.DisplayDialog("Thing World",
                        "This is a model file, not a prefab. Make it a city piece first (menu 5), then make that prefab interactive.", "OK");
                    return;
                }
                GameObject root = PrefabUtility.LoadPrefabContents(assetPath);
                try
                {
                    PrepareInteractive(root, undo: false);
                    PrefabUtility.SaveAsPrefabAsset(root, assetPath);
                }
                finally { PrefabUtility.UnloadPrefabContents(root); }
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(assetPath));
            }
            else
            {
                PrepareInteractive(go, undo: true);
                EditorSceneManager.MarkSceneDirty(go.scene);
            }
            Debug.Log($"[Thing World] '{go.name}' is now interactive. Set WHEN and DO in the Inspector (World Interactable).", go);
        }

        /// <summary>Adds a WorldInteractable and clears static flags (moving objects can't be static).</summary>
        static void PrepareInteractive(GameObject go, bool undo)
        {
            if (go.GetComponent<WorldInteractable>() == null)
            {
                if (undo) Undo.AddComponent<WorldInteractable>(go);
                else go.AddComponent<WorldInteractable>();
            }
            foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            {
                if (undo) Undo.RecordObject(t.gameObject, "Clear static");
                GameObjectUtility.SetStaticEditorFlags(t.gameObject, (StaticEditorFlags)0);
            }
            if (go.GetComponentsInChildren<Collider>(true).Length == 0)
            {
                Bounds b = LocalBounds(go);
                var box = undo ? Undo.AddComponent<BoxCollider>(go) : go.AddComponent<BoxCollider>();
                box.center = b.center;
                box.size = b.size;
            }
        }

        [MenuItem("Thing World/World Building/Interactive: Add Simple Mover To Selected (doors, lifts)", priority = 160)]
        static void AddMover()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null || !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go)))
            {
                EditorUtility.DisplayDialog("Thing World",
                    "Click the moving PART in the Hierarchy (for a door: the door panel, not the whole house). " +
                    "Open the prefab first if it is a prefab.", "OK");
                return;
            }
            if (go.GetComponent<SimpleMover>() == null) Undo.AddComponent<SimpleMover>(go);
            foreach (Transform t in go.GetComponentsInChildren<Transform>(true))
            {
                Undo.RecordObject(t.gameObject, "Clear static");
                GameObjectUtility.SetStaticEditorFlags(t.gameObject, (StaticEditorFlags)0);
            }
            EditorSceneManager.MarkSceneDirty(go.scene);
            Debug.Log($"[Thing World] Simple Mover added to '{go.name}'. Drag it into a World Interactable's 'Mover' slot.", go);
        }

        // ==================================================================
        //  Ground
        // ==================================================================

        [MenuItem(Menu + "Ground: Make Ground From Selected Model", priority = 100)]
        static void MakeGround()
        {
            if (!SelectedModelInOwnFolder(out GameObject model, out string name)) return;
            string folder = Role(name, "Ground");
            string prefabPath = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{Prefixed(name, model.name)}.prefab");

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(model);
            PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            instance.name = Path.GetFileNameWithoutExtension(prefabPath);
            instance.transform.position = Vector3.zero;
            StripCamerasAndLights(instance);
            AddMeshCollidersAndStatic(instance);

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
            Object.DestroyImmediate(instance);
            Ping(prefab);
            EditorUtility.DisplayDialog("Thing World — Ground",
                $"Ground prefab made:\n{prefabPath}\n\n" +
                "Model it in Blender in METERS with the city center at (0,0,0).\n" +
                "Re-export the FBX with the same name and this prefab updates.\n\n" +
                "To walk on it: Thing World → World Building → Open My World Test Scene.", "OK");
        }

        // ==================================================================
        //  Sky
        // ==================================================================

        [MenuItem(Menu + "Sky: Create Sky Material", priority = 110)]
        static void CreateSky()
        {
            ThingWorldPaths.WithCreator("Create Sky", name =>
            {
                Shader shader = Shader.Find("ThingWorld/Sky");
                if (shader == null) { EditorUtility.DisplayDialog("Thing World", "The ThingWorld/Sky shader is missing. Import the latest Thing World update.", "OK"); return; }
                string path = AssetDatabase.GenerateUniqueAssetPath($"{Role(name, "Sky")}/{name}_Sky.mat");
                var mat = new Material(shader) { name = Path.GetFileNameWithoutExtension(path) };
                AssetDatabase.CreateAsset(mat, path);
                AssetDatabase.SaveAssets();
                ApplySky(mat);
                Ping(mat);
                EditorUtility.DisplayDialog("Thing World — Sky",
                    $"Sky material made:\n{path}\n\n" +
                    "It is now the sky of the scene you have open. Change its colors in the Inspector.\n" +
                    "Optional: drop a painted 360° panorama (2:1 image, max 4096×2048) into 'Panorama' and raise 'Panorama strength'.\n\n" +
                    "Press Play to see it go from day to night.", "OK");
            });
        }

        static void ApplySky(Material mat)
        {
            RenderSettings.skybox = mat;
            DynamicGI.UpdateEnvironment();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        // ==================================================================
        //  Lighting
        // ==================================================================

        [MenuItem(Menu + "Lighting: Create Lighting Settings", priority = 120)]
        static void CreateLighting()
        {
            ThingWorldPaths.WithCreator("Create Lighting", name =>
            {
                string path = AssetDatabase.GenerateUniqueAssetPath($"{Role(name, "Lighting")}/{name}_Lighting.asset");
                var asset = ScriptableObject.CreateInstance<WorldLighting>();
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
                ApplyLighting(asset);
                Ping(asset);
                EditorUtility.DisplayDialog("Thing World — Lighting",
                    $"Lighting settings made:\n{path}\n\n" +
                    "It is connected to the Sun of the scene you have open.\n" +
                    "Press Play, then change the colors in the Inspector — you see the result live, and changes to this asset are KEPT after Play.", "OK");
            });
        }

        static void ApplyLighting(WorldLighting asset)
        {
            var cycle = Object.FindFirstObjectByType<DayNightCycle>();
            if (cycle == null) { Debug.LogWarning("[Thing World] No Sun (Day Night Cycle) in this scene."); return; }
            Undo.RecordObject(cycle, "Assign lighting");
            cycle.lighting = asset;
            EditorUtility.SetDirty(cycle);
            EditorSceneManager.MarkSceneDirty(cycle.gameObject.scene);
        }

        [MenuItem(Menu + "Lighting: Create Lamp (point light)", priority = 121)]
        static void CreateLamp()
        {
            ThingWorldPaths.WithCreator("Create Lamp", name =>
            {
                string path = AssetDatabase.GenerateUniqueAssetPath($"{Role(name, "Lighting")}/{name}_Lamp.prefab");
                var go = new GameObject(Path.GetFileNameWithoutExtension(path));
                var light = go.AddComponent<Light>();
                light.type = LightType.Point;
                light.range = 8f;
                light.intensity = 2f;
                light.color = new Color(1f, 0.85f, 0.6f);
                light.shadows = LightShadows.None; // shadows from extra lights are too expensive on Quest
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
                Object.DestroyImmediate(go);
                Ping(prefab);
                EditorUtility.DisplayDialog("Thing World — Lamp",
                    $"Lamp made:\n{path}\n\nYou may make up to {Cfg.maxLampsPerStudent} lamps for the whole city. " +
                    "No shadows, range under 15 m. Glowing (emissive) materials are free — use those for everything else.", "OK");
            });
        }

        // ==================================================================
        //  Particles
        // ==================================================================

        [MenuItem(Menu + "Particles: Create Particle Effect", priority = 130)]
        static void CreateParticles()
        {
            ThingWorldPaths.WithCreator("Create Particles", name =>
            {
                string folder = Role(name, "Particles");
                string path = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{name}_Particles.prefab");
                string matPath = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{name}_ParticleMaterial.mat");

                Material source = Cfg.sparkMaterial;
                Material mat = source != null ? new Material(source) : new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
                AssetDatabase.CreateAsset(mat, matPath);

                var go = new GameObject(Path.GetFileNameWithoutExtension(path));
                var ps = go.AddComponent<ParticleSystem>();
                var main = ps.main;
                main.maxParticles = 200;
                main.startLifetime = 3f;
                main.startSpeed = 0.5f;
                main.startSize = 0.08f;
                main.loop = true;
                var emission = ps.emission;
                emission.rateOverTime = 30f;
                var shape = ps.shape;
                shape.shapeType = ParticleSystemShapeType.Box;
                shape.scale = new Vector3(4f, 0.5f, 4f);
                go.GetComponent<ParticleSystemRenderer>().sharedMaterial = mat;
                go.AddComponent<DayNightParticles>(); // "Show When": always / day / night / sunrise+sunset

                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
                Object.DestroyImmediate(go);
                Ping(prefab);
                EditorUtility.DisplayDialog("Thing World — Particles",
                    $"Particle effect made:\n{path}\n\n" +
                    $"Budget: up to {Cfg.particleMaxPerSystem} particles per effect (Max Particles), {Cfg.particleMaxPerStudent} for all your effects together.\n" +
                    "Small, slow, few particles look best in VR.\n\n" +
                    "'Day Night Particles' → Show When: make it appear only at night, only in the day, or at sunrise/sunset.", "OK");
            });
        }

        // ==================================================================
        //  Sound
        // ==================================================================

        [MenuItem(Menu + "Sound: Make Ambient Sound From Selected Clip (3D, in one place)", priority = 140)]
        static void MakeAmbientSound() => MakeSound(spatial: true);

        [MenuItem(Menu + "Sound: Make Background Sound From Selected Clip (everywhere)", priority = 141)]
        static void MakeBackgroundSound() => MakeSound(spatial: false);

        static void MakeSound(bool spatial)
        {
            var clip = Selection.activeObject as AudioClip;
            if (clip == null)
            {
                EditorUtility.DisplayDialog("Thing World",
                    "First click your sound file (.wav / .mp3 / .ogg) in the Project window so it is highlighted, then use this menu.\n\n" +
                    "Tip: put sound files in Assets/Creators/YOURNAME/Sounds.", "OK");
                return;
            }
            string clipPath = AssetDatabase.GetAssetPath(clip);
            string owner = OwnerOf(clipPath);
            if (owner != null) { BuildSound(clip, clipPath, owner, spatial); return; }

            // The clip is outside every student folder: move it into the student's Sounds folder first.
            ThingWorldPaths.WithCreator("Make Sound", name =>
            {
                string folder = ThingWorldPaths.SoundsFolder(name);
                ThingWorldPaths.EnsureFolder(folder);
                string target = AssetDatabase.GenerateUniqueAssetPath($"{folder}/{Path.GetFileName(clipPath)}");
                string error = AssetDatabase.MoveAsset(clipPath, target);
                if (!string.IsNullOrEmpty(error)) { EditorUtility.DisplayDialog("Thing World", "Couldn't move the sound into your folder:\n" + error, "OK"); return; }
                BuildSound(clip, target, name, spatial);
            });
        }

        static void BuildSound(AudioClip clip, string clipPath, string name, bool spatial)
        {
            string kind = spatial ? "Ambient" : "Background";
            string path = AssetDatabase.GenerateUniqueAssetPath($"{Role(name, "Sounds")}/{Prefixed(name, clip.name)}_{kind}.prefab");

            var go = new GameObject(Path.GetFileNameWithoutExtension(path));
            var a = go.AddComponent<AudioSource>();
            a.clip = clip;
            a.loop = true;
            a.playOnAwake = true;
            a.spatialBlend = spatial ? 1f : 0f;
            a.volume = spatial ? 0.8f : 0.3f;
            a.rolloffMode = AudioRolloffMode.Linear;
            a.minDistance = 2f;
            a.maxDistance = 25f;

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);

            // Long looping clips should stream instead of filling memory.
            if (AssetImporter.GetAtPath(clipPath) is AudioImporter imp && clip.length > 10f)
            {
                AudioImporterSampleSettings s = imp.defaultSampleSettings;
                s.loadType = AudioClipLoadType.Streaming;
                imp.defaultSampleSettings = s;
                imp.SaveAndReimport();
            }
            Ping(prefab);
            EditorUtility.DisplayDialog("Thing World — Sound",
                spatial
                    ? $"Ambient sound made:\n{path}\n\nPlace it in the world: you hear it when you are within {a.maxDistance:0} m."
                    : $"Background sound made:\n{path}\n\nIt plays everywhere at the same volume. The city should have only ONE of these.", "OK");
        }

        // ==================================================================
        //  Shared materials (textures coordinators)
        // ==================================================================

        [MenuItem(Menu + "Shared: Create Shared Material", priority = 150)]
        static void CreateSharedMaterial()
        {
            ThingWorldPaths.WithCreator("Create Shared Material", name =>
            {
                string folder = SharedFolderOf(name) + "/Materials";
                ThingWorldPaths.EnsureFolder(folder);
                ThingWorldPaths.EnsureFolder(SharedFolderOf(name) + "/Textures");
                string path = AssetDatabase.GenerateUniqueAssetPath($"{folder}/Shared_{name}_Material.mat");
                var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(mat, path);
                AssetDatabase.SaveAssets();
                Ping(mat);
                EditorUtility.DisplayDialog("Thing World — Shared Material",
                    $"Made:\n{path}\n\nRename it (e.g. Shared_{name}_Brick), put its textures in {SharedFolderOf(name)}/Textures (max {Cfg.maxTextureSize}px), " +
                    "and set Tiling so it looks right at real scale.\n\nWhen ready: World Building → Shared: Export My Shared Library.", "OK");
            });
        }

        [MenuItem(Menu + "Shared: Export My Shared Library", priority = 151)]
        static void ExportShared()
        {
            ThingWorldPaths.WithCreator("Export Shared Library", name =>
            {
                string folder = SharedFolderOf(name);
                if (!AssetDatabase.IsValidFolder(folder)) { EditorUtility.DisplayDialog("Thing World", $"{folder} doesn't exist yet.", "OK"); return; }
                ExportFolder(folder, $"Shared_{name}");
            });
        }

        [MenuItem("Thing World/Instructor/Export Shared Library For Everyone", priority = 232)]
        static void ExportAllShared()
        {
            if (!AssetDatabase.IsValidFolder(ThingWorldPaths.SharedFolder)) { EditorUtility.DisplayDialog("Thing World", "There is no Assets/Shared folder yet.", "OK"); return; }
            ExportFolder(ThingWorldPaths.SharedFolder, "ThingWorld_SharedLibrary");
        }

        static string SharedFolderOf(string name) => $"{ThingWorldPaths.SharedFolder}/{name}";

        static void ExportFolder(string folder, string fileStem)
        {
            AssetDatabase.SaveAssets();
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string file = Path.Combine(desktop, $"{fileStem}_{DateTime.Now:yyyy-MM-dd_HHmm}.unitypackage");
            AssetDatabase.ExportPackage(folder, file, ExportPackageOptions.Recurse);
            EditorUtility.RevealInFinder(file);
            EditorUtility.DisplayDialog("Thing World", $"Exported to your Desktop:\n{Path.GetFileName(file)}", "OK");
        }

        // ==================================================================
        //  World test scene (any student)
        // ==================================================================

        [MenuItem(Menu + "Open My World Test Scene", priority = 170)]
        static void OpenWorldTest()
        {
            ThingWorldPaths.WithCreator("World Test Scene", name =>
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
                string path = ThingWorldPaths.WorldTestScenePath(name);
                if (File.Exists(path)) EditorSceneManager.OpenScene(path);
                else BuildWorldTest(name);
            });
        }

        [MenuItem(Menu + "Rebuild My World Test Scene", priority = 171)]
        static void RebuildWorldTest()
        {
            ThingWorldPaths.WithCreator("World Test Scene", name =>
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
                BuildWorldTest(name);
            });
        }

        static void BuildWorldTest(string name)
        {
            string path = ThingWorldPaths.WorldTestScenePath(name);
            Scene scene = ThingWorldSetup.BuildScene(path, groundSize: 60f, withDummies: true, secondsPerDay: 120f, spawnRadius: 12f);
            ApplyWorldWork(new[] { name }, layOutEverything: true);
            EditorSceneManager.SaveScene(scene);
            EditorUtility.DisplayDialog("Thing World — World Test Scene",
                "This scene has your sky, lighting, ground and all your world prefabs, plus your Thing and 3 blobs.\n\n" +
                "Press Play to walk around (WASD + right mouse in the simulator).\n" +
                "Changed something? Use 'Rebuild My World Test Scene'.", "OK");
        }

        /// <summary>
        /// Puts the students' world work into the open scene: first sky and first lighting found,
        /// every ground prefab at (0,0,0). With layOutEverything, also places all other world prefabs in a row.
        /// Returns a short report.
        /// </summary>
        static string ApplyWorldWork(IEnumerable<string> names, bool layOutEverything)
        {
            var report = new List<string>();
            names = names.ToList();

            List<Material> skies = names.SelectMany(n => Load<Material>(Role(n, "Sky", create: false), "t:Material"))
                                        .Where(m => m.shader != null && (m.shader.name == "ThingWorld/Sky" || m.shader.name.StartsWith("Skybox/")))
                                        .ToList();
            if (skies.Count > 0)
            {
                ApplySky(skies[0]);
                report.Add($"Sky: {skies[0].name}" + (skies.Count > 1 ? $"  (also found: {string.Join(", ", skies.Skip(1).Select(s => s.name))})" : ""));
            }
            else report.Add("Sky: none yet (default sky)");

            List<WorldLighting> lights = names.SelectMany(n => Load<WorldLighting>(Role(n, "Lighting", create: false), "t:WorldLighting")).ToList();
            if (lights.Count > 0)
            {
                ApplyLighting(lights[0]);
                report.Add($"Lighting: {lights[0].name}" + (lights.Count > 1 ? $"  (also found: {string.Join(", ", lights.Skip(1).Select(s => s.name))})" : ""));
            }
            else report.Add("Lighting: none yet (simple day/night)");

            List<GameObject> grounds = names.SelectMany(n => Load<GameObject>(Role(n, "Ground", create: false), "t:Prefab")).ToList();
            int placedGrounds = 0;
            foreach (GameObject g in grounds)
            {
                if (InScene(g)) continue;
                var inst = (GameObject)PrefabUtility.InstantiatePrefab(g);
                inst.transform.position = Vector3.zero;
                placedGrounds++;
            }
            if (grounds.Count > 0)
            {
                GameObject placeholder = GameObject.Find("Ground (placeholder)");
                if (placeholder != null) placeholder.SetActive(false);
            }
            report.Add($"Ground: {grounds.Count} prefab(s), {placedGrounds} newly placed");

            if (layOutEverything)
            {
                var others = new List<GameObject>();
                foreach (string n in names)
                {
                    foreach (string role in new[] { "Lighting", "Particles", "Sounds", "Interactive" })
                        others.AddRange(Load<GameObject>(Role(n, role, create: false), "t:Prefab"));
                    others.AddRange(Load<GameObject>(ThingWorldPaths.PiecesFolder(n), "t:Prefab"));
                }
                float x = -others.Count * 2f;
                foreach (GameObject p in others)
                {
                    var inst = (GameObject)PrefabUtility.InstantiatePrefab(p);
                    inst.transform.position = new Vector3(x, inst.GetComponent<Light>() != null ? 2.5f : 0f, 6f);
                    x += 4f;
                }
                report.Add($"Other world prefabs placed in a row: {others.Count}");
            }
            return string.Join("\n", report);
        }

        // ==================================================================
        //  Instructor: assemble the city
        // ==================================================================

        [MenuItem("Thing World/Instructor/Assemble City (sky, lighting, ground)", priority = 212)]
        static void AssembleCity()
        {
            if (!File.Exists(ThingWorldPaths.WorldScenePath))
            {
                EditorUtility.DisplayDialog("Thing World", "No city scene yet. Run 'Instructor → Create City Scene' first.", "OK");
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            Scene scene = EditorSceneManager.OpenScene(ThingWorldPaths.WorldScenePath);
            string report = ApplyWorldWork(ThingWorldPaths.AllCreators(), layOutEverything: false);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("[Thing World] City assembled:\n" + report);
            EditorUtility.DisplayDialog("Thing World — City assembled", report +
                "\n\nNext: drag pieces, lamps, particles, sounds and interactive objects from each student's folder into the city, " +
                "then Instructor → Bake City NavMesh.", "OK");
        }

        // ==================================================================
        //  helpers
        // ==================================================================

        static ThingWorldConfig Cfg => ThingWorldConfig.Load() ?? ScriptableObject.CreateInstance<ThingWorldConfig>();

        static string Role(string name, string role, bool create = true)
        {
            string f = ThingWorldPaths.RoleFolder(name, role);
            if (create) ThingWorldPaths.EnsureFolder(f);
            return f;
        }

        static string Prefixed(string name, string n) => n.StartsWith(name + "_", StringComparison.Ordinal) ? n : $"{name}_{n}";

        static string OwnerOf(string assetPath) =>
            ThingWorldPaths.AllCreators().FirstOrDefault(n => ThingWorldPaths.IsInside(assetPath, ThingWorldPaths.CreatorFolder(n)));

        static bool SelectedModelInOwnFolder(out GameObject model, out string name)
        {
            model = Selection.activeObject as GameObject;
            string path = model != null ? AssetDatabase.GetAssetPath(model) : null;
            name = string.IsNullOrEmpty(path) ? null : OwnerOf(path);
            if (string.IsNullOrEmpty(path))
            {
                EditorUtility.DisplayDialog("Thing World", "First click your model (FBX) in the Project window, then use this menu.", "OK");
                return false;
            }
            if (name == null)
            {
                EditorUtility.DisplayDialog("Thing World", "Put the model inside your own folder first, e.g.\nAssets/Creators/YourName/Pieces", "OK");
                return false;
            }
            return true;
        }

        static IEnumerable<T> Load<T>(string folder, string filter) where T : Object
        {
            if (!AssetDatabase.IsValidFolder(folder)) yield break;
            foreach (string guid in AssetDatabase.FindAssets(filter, new[] { folder }))
            {
                T a = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                if (a != null) yield return a;
            }
        }

        static bool InScene(GameObject prefab)
        {
            foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
                if (PrefabUtility.GetCorrespondingObjectFromSource(root) == prefab) return true;
            return false;
        }

        static void StripCamerasAndLights(GameObject go)
        {
            foreach (Camera c in go.GetComponentsInChildren<Camera>(true)) Object.DestroyImmediate(c.gameObject);
            foreach (Light l in go.GetComponentsInChildren<Light>(true)) Object.DestroyImmediate(l.gameObject);
        }

        public static void AddMeshCollidersAndStatic(GameObject root)
        {
            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
            {
                GameObject g = t.gameObject;
                MeshFilter mf = g.GetComponent<MeshFilter>();
                if (mf != null && mf.sharedMesh != null && g.GetComponent<Collider>() == null)
                    g.AddComponent<MeshCollider>().sharedMesh = mf.sharedMesh;
                if (g.GetComponent<Renderer>() != null)
                    GameObjectUtility.SetStaticEditorFlags(g, StaticEditorFlags.BatchingStatic | StaticEditorFlags.ContributeGI |
                                                              StaticEditorFlags.OccluderStatic | StaticEditorFlags.OccludeeStatic);
            }
        }

        static Bounds LocalBounds(GameObject go)
        {
            Renderer[] rends = go.GetComponentsInChildren<Renderer>(true);
            if (rends.Length == 0) return new Bounds(Vector3.zero, Vector3.one * 0.5f);
            Bounds world = rends[0].bounds;
            foreach (Renderer r in rends) world.Encapsulate(r.bounds);
            Vector3 c = go.transform.InverseTransformPoint(world.center);
            Vector3 s = go.transform.InverseTransformVector(world.size);
            return new Bounds(c, new Vector3(Mathf.Abs(s.x), Mathf.Abs(s.y), Mathf.Abs(s.z)));
        }

        static void Ping(Object o)
        {
            Selection.activeObject = o;
            EditorGUIUtility.PingObject(o);
        }
    }
}
