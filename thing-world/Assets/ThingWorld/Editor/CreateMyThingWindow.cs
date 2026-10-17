using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ThingWorldEditor
{
    /// <summary>
    /// "Thing World → 1. Create or Update My Thing"
    /// Turns a student's character model into a working Thing: folders, script, animation slots,
    /// prefab with every component configured, and a test scene to press Play in.
    /// </summary>
    public class CreateMyThingWindow : EditorWindow
    {
        const string PendingKey = "ThingWorld.PendingAttach";

        string creatorName = "";
        GameObject model;
        AnimationClip idleClip;
        AnimationClip walkClip;
        Vector2 scroll;

        [MenuItem("Thing World/1. Create or Update My Thing", priority = 0)]
        static void Open()
        {
            var w = GetWindow<CreateMyThingWindow>(true, "Create My Thing");
            w.minSize = new Vector2(440, 430);
            var creators = ThingWorldPaths.AllCreators();
            if (creators.Count == 1) w.creatorName = creators[0];
        }

        void OnGUI()
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);

            EditorGUILayout.LabelField("Create or Update My Thing", EditorStyles.largeLabel);
            EditorGUILayout.HelpBox(
                "1. Type your first name (letters only).\n" +
                "2. Put your character's FBX into  Assets/Creators/YourName/Character\n" +
                "   (click 'Make My Folders' first if the folder doesn't exist yet).\n" +
                "3. Drag the character into 'My Character'.\n" +
                "4. Optional: drag an Idle and a Walk animation (Mixamo, 'In Place').\n" +
                "5. Click the big button. Running it again later updates your Thing and keeps your script.",
                MessageType.Info);

            EditorGUILayout.Space();
            creatorName = EditorGUILayout.TextField("My first name", creatorName);
            string clean = ThingWorldPaths.CleanName(creatorName);
            if (!string.IsNullOrEmpty(creatorName) && clean == null)
                EditorGUILayout.HelpBox("Use letters and numbers only, starting with a letter.", MessageType.Warning);
            else if (clean != null)
                EditorGUILayout.LabelField("Your folder", ThingWorldPaths.CreatorFolder(clean), EditorStyles.miniLabel);

            using (new EditorGUI.DisabledScope(clean == null))
            {
                if (GUILayout.Button("Make My Folders"))
                {
                    ThingWorldPaths.EnsureCreatorFolders(clean);
                    AssetDatabase.Refresh();
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<DefaultAsset>(ThingWorldPaths.CharacterFolder(clean)));
                }
            }

            EditorGUILayout.Space();
            model = (GameObject)EditorGUILayout.ObjectField("My Character", model, typeof(GameObject), false);
            idleClip = (AnimationClip)EditorGUILayout.ObjectField("Idle animation (optional)", idleClip, typeof(AnimationClip), false);
            walkClip = (AnimationClip)EditorGUILayout.ObjectField("Walk animation (optional)", walkClip, typeof(AnimationClip), false);

            string problem = Problem(clean);
            if (problem != null) EditorGUILayout.HelpBox(problem, MessageType.Warning);

            EditorGUILayout.Space();
            bool exists = clean != null && File.Exists(ThingWorldPaths.PrefabPath(clean));
            using (new EditorGUI.DisabledScope(problem != null))
            {
                if (GUILayout.Button(exists ? "Update My Thing" : "Create My Thing", GUILayout.Height(40)))
                {
                    Build(clean, model, idleClip, walkClip);
                }
            }

            EditorGUILayout.EndScrollView();
        }

        string Problem(string clean)
        {
            if (clean == null) return "Type your name first.";
            if (model == null) return "Drag your character model into 'My Character'.";
            string path = AssetDatabase.GetAssetPath(model);
            if (!ThingWorldPaths.IsInside(path, ThingWorldPaths.CreatorFolder(clean)))
                return $"Your character must be inside your own folder:\n{ThingWorldPaths.CharacterFolder(clean)}\n\nDrag the FBX there in the Project window, then drag it in here again.";
            if (ThingWorldPaths.IsInside(path, ThingWorldPaths.ThingsFolder(clean)))
                return "Use your original FBX from the Character folder, not the Thing prefab.";
            return null;
        }

        // ------------------------------------------------------------------

        static void Build(string name, GameObject model, AnimationClip idle, AnimationClip walk)
        {
            var config = ThingWorldConfig.Load();
            if (config == null || config.baseAnimator == null)
            {
                EditorUtility.DisplayDialog("Thing World", "This project is not set up yet.\nAsk your instructor (Thing World → Instructor → Set Up Project).", "OK");
                return;
            }

            ThingWorldPaths.EnsureCreatorFolders(name);
            string modelPath = AssetDatabase.GetAssetPath(model);

            // 1. Rig: humanoid, so Mixamo and motion capture animations can be shared.
            if (!MakeHumanoidIfNeeded(modelPath)) return;
            model = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);

            // 2. Script (only created the first time — never overwrites student code)
            string className = ThingWorldPaths.ClassName(name);
            bool newScript = false;
            if (!File.Exists(ThingWorldPaths.ScriptPath(name)))
            {
                string templatePath = ThingWorldPaths.FindTemplatePath();
                if (templatePath == null)
                {
                    EditorUtility.DisplayDialog("Thing World", "ThingTemplate.cs.txt is missing from Assets/ThingWorld.", "OK");
                    return;
                }
                string code = File.ReadAllText(templatePath)
                    .Replace("#CLASSNAME#", className)
                    .Replace("#CREATOR#", name);
                File.WriteAllText(ThingWorldPaths.ScriptPath(name), code);
                newScript = true;
            }

            // 3. Animation slots
            AnimatorOverrideController overrides = MakeOverrides(name, config.baseAnimator, idle, walk);

            // 4. Prefab
            BuildPrefab(name, model, overrides);

            // 5. Thing script on the prefab — needs Unity to compile the new script first.
            SessionState.SetString(PendingKey, name);
            if (newScript)
            {
                AssetDatabase.ImportAsset(ThingWorldPaths.ScriptPath(name), ImportAssetOptions.ForceUpdate);
                AssetDatabase.Refresh();
                // Unity now compiles the new script; FinishAfterCompile() continues after the reload.
            }
            else
            {
                FinishAfterCompile();
            }
        }

        [DidReloadScripts]
        static void FinishAfterCompile()
        {
            string name = SessionState.GetString(PendingKey, "");
            if (string.IsNullOrEmpty(name)) return;

            Type type = ThingWorldPaths.FindThingType(ThingWorldPaths.ClassName(name));
            if (type == null)
            {
                if (EditorApplication.isCompiling) return; // try again after compiling finishes
                SessionState.EraseString(PendingKey);
                EditorUtility.DisplayDialog("Thing World",
                    $"Unity couldn't find the class '{ThingWorldPaths.ClassName(name)}'.\n\n" +
                    "Is there a red error in the Console? Fix it, then click 'Update My Thing' again.", "OK");
                return;
            }
            SessionState.EraseString(PendingKey);

            string prefabPath = ThingWorldPaths.PrefabPath(name);
            GameObject root = PrefabUtility.LoadPrefabContents(prefabPath);
            try
            {
                foreach (Thing other in root.GetComponents<Thing>())
                    if (other.GetType() != type) UnityEngine.Object.DestroyImmediate(other, true);
                if (root.GetComponent(type) == null) root.AddComponent(type);
                PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            OpenOrBuildTestScene(name, rebuild: true);

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);

            EditorUtility.DisplayDialog("Thing World — Done!",
                $"Your Thing is ready.\n\n" +
                $"• Your script: {ThingWorldPaths.ScriptPath(name)}\n" +
                $"• Your test scene is open. Press Play to meet your Thing.\n" +
                $"• Sounds: select your prefab and drag audio files into 'My Sounds'.\n" +
                $"• Animations: double-click {name}_Animations in your Animation folder.", "Let's go");
        }

        // ------------------------------------------------------------------

        static bool MakeHumanoidIfNeeded(string modelPath)
        {
            var importer = AssetImporter.GetAtPath(modelPath) as ModelImporter;
            if (importer == null) return true; // a prefab, not a model file

            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);
            bool hasSkeleton = asset.GetComponentInChildren<SkinnedMeshRenderer>(true) != null;
            if (!hasSkeleton || importer.animationType == ModelImporterAnimationType.Human) return true;

            int choice = EditorUtility.DisplayDialogComplex("Thing World",
                "Your character has a skeleton. Set it up as Humanoid?\n\n" +
                "Humanoid lets you use Mixamo animations now and your own motion capture later.",
                "Yes, make it Humanoid", "Cancel", "No, keep it as is");
            if (choice == 1) return false;
            if (choice == 2) return true;

            importer.animationType = ModelImporterAnimationType.Human;
            importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            importer.SaveAndReimport();

            Avatar avatar = null;
            foreach (UnityEngine.Object o in AssetDatabase.LoadAllAssetsAtPath(modelPath))
                if (o is Avatar a) avatar = a;
            if (avatar == null || !avatar.isHuman)
            {
                EditorUtility.DisplayDialog("Thing World",
                    "Unity couldn't map your skeleton as a human. Your Thing will still work, but animations won't play.\n\n" +
                    "Tip: rig the character with Mixamo (auto-rigger) and import the FBX it gives you.", "OK");
            }
            return true;
        }

        static AnimatorOverrideController MakeOverrides(string name, RuntimeAnimatorController baseAnimator,
                                                        AnimationClip idle, AnimationClip walk)
        {
            string path = ThingWorldPaths.OverridePath(name);
            var aoc = AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(path);
            if (aoc == null)
            {
                aoc = new AnimatorOverrideController(baseAnimator) { name = $"{name}_Animations" };
                AssetDatabase.CreateAsset(aoc, path);
            }
            if (idle != null) aoc["Idle"] = idle;
            if (walk != null) aoc["Walk"] = walk;
            EditorUtility.SetDirty(aoc);
            AssetDatabase.SaveAssets();
            return aoc;
        }

        static void BuildPrefab(string name, GameObject model, AnimatorOverrideController overrides)
        {
            string prefabPath = ThingWorldPaths.PrefabPath(name);
            bool exists = File.Exists(prefabPath);
            GameObject root = exists ? PrefabUtility.LoadPrefabContents(prefabPath) : new GameObject(ThingWorldPaths.ClassName(name));

            try
            {
                // Replace the body, keep everything else (script settings, sounds).
                Transform old = root.transform.Find("Model");
                if (old != null) UnityEngine.Object.DestroyImmediate(old.gameObject);

                var body = (GameObject)PrefabUtility.InstantiatePrefab(model, root.scene);
                PrefabUtility.UnpackPrefabInstance(body, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
                body.name = "Model";
                body.transform.SetParent(root.transform, false);
                body.transform.localPosition = Vector3.zero;
                body.transform.localRotation = Quaternion.identity;

                // Things exported from Blender sometimes bring a camera or light along.
                foreach (Camera c in body.GetComponentsInChildren<Camera>(true)) UnityEngine.Object.DestroyImmediate(c.gameObject);
                foreach (Light l in body.GetComponentsInChildren<Light>(true)) UnityEngine.Object.DestroyImmediate(l.gameObject);
                foreach (Collider col in body.GetComponentsInChildren<Collider>(true)) UnityEngine.Object.DestroyImmediate(col);

                // Size check: Blender exports at the wrong scale are common.
                Bounds b = LocalBounds(root.transform, body);
                float height = b.size.y;
                if (height > 3.5f || height < 0.25f)
                {
                    if (EditorUtility.DisplayDialog("Thing World",
                        $"Your character is {height:0.##} m tall. That's probably an export-scale problem.\n\nResize it to 1.7 m (human size)?",
                        "Resize to 1.7 m", "Keep my size"))
                    {
                        body.transform.localScale *= 1.7f / Mathf.Max(0.0001f, height);
                        b = LocalBounds(root.transform, body);
                    }
                }

                // Feet on the ground: move the body so its lowest point is at the root.
                body.transform.localPosition = new Vector3(-b.center.x, -b.min.y, -b.center.z);
                b = LocalBounds(root.transform, body);

                Animator animator = body.GetComponentInChildren<Animator>();
                if (animator == null) animator = body.AddComponent<Animator>();
                animator.runtimeAnimatorController = overrides;
                animator.applyRootMotion = false;
                animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;

                ThingWorldSetup.ConfigureThingRoot(root, b);
                PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
            }
            finally
            {
                if (exists) PrefabUtility.UnloadPrefabContents(root);
                else UnityEngine.Object.DestroyImmediate(root);
            }
        }

        static Bounds LocalBounds(Transform root, GameObject body)
        {
            var rends = body.GetComponentsInChildren<Renderer>(true);
            if (rends.Length == 0) return new Bounds(new Vector3(0f, 0.85f, 0f), new Vector3(0.5f, 1.7f, 0.5f));
            Bounds b = new Bounds();
            bool first = true;
            foreach (Renderer r in rends)
            {
                Bounds wb = r.bounds;
                // Convert the 8 corners into the root's local space.
                for (int i = 0; i < 8; i++)
                {
                    Vector3 corner = new Vector3(
                        (i & 1) == 0 ? wb.min.x : wb.max.x,
                        (i & 2) == 0 ? wb.min.y : wb.max.y,
                        (i & 4) == 0 ? wb.min.z : wb.max.z);
                    Vector3 local = root.InverseTransformPoint(corner);
                    if (first) { b = new Bounds(local, Vector3.zero); first = false; }
                    else b.Encapsulate(local);
                }
            }
            return b;
        }

        // ------------------------------------------------------------------

        [MenuItem("Thing World/2. Open My Test Scene", priority = 1)]
        static void OpenMyTestScene()
        {
            ThingWorldPaths.WithCreator("Open Test Scene", name =>
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
                OpenOrBuildTestScene(name, rebuild: false);
            });
        }

        [MenuItem("Thing World/Rebuild My Test Scene", priority = 2)]
        static void RebuildMyTestScene()
        {
            ThingWorldPaths.WithCreator("Rebuild Test Scene", name =>
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
                OpenOrBuildTestScene(name, rebuild: true);
            });
        }

        static void OpenOrBuildTestScene(string name, bool rebuild)
        {
            string path = ThingWorldPaths.TestScenePath(name);
            if (!rebuild && File.Exists(path))
            {
                EditorSceneManager.OpenScene(path);
                return;
            }

            var scene = ThingWorldSetup.BuildScene(path, groundSize: 40f, withDummies: true, secondsPerDay: 120f, spawnRadius: 6f);

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(ThingWorldPaths.PrefabPath(name));
            if (prefab != null)
            {
                var me = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
                me.transform.position = new Vector3(0f, 0f, 1f);
                me.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // facing the player
            }
            EditorSceneManager.SaveScene(scene);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(path, true) };
        }
    }
}
