using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace ThingWorldEditor
{
    /// <summary>Instructor tools: one-time project setup, building scenes, baking the city.</summary>
    public static class ThingWorldSetup
    {
        // ------------------------------------------------------------------
        //  ONE-TIME PROJECT SETUP
        // ------------------------------------------------------------------

        [MenuItem("Thing World/Instructor/Set Up Project (run once)", priority = 200)]
        public static void SetUpProject()
        {
            var report = new StringBuilder();

            ThingWorldPaths.EnsureFolder(ThingWorldPaths.Generated);
            ThingWorldPaths.EnsureFolder(ThingWorldPaths.Placeholders);
            ThingWorldPaths.EnsureFolder(ThingWorldPaths.ConfigFolder);
            ThingWorldPaths.EnsureFolder(ThingWorldPaths.CreatorsRoot);

            Material spark = CreateSparkMaterial();
            AnimatorController animator = CreateBaseAnimator();
            ThingWorldConfig config = CreateOrUpdateConfig(spark, animator);
            CreateDummyPrefab();
            report.AppendLine("✔ Spark material, base animator, config and test dummy created.");

            if (EnsureTeleportLayer()) report.AppendLine("✔ Layer 31 named 'Teleport'.");

            if (Resources.Load<TMPro.TMP_Settings>("TMP Settings") == null)
                report.AppendLine("✖ TextMeshPro resources missing → Window → TextMeshPro → Import TMP Essential Resources.");
            else report.AppendLine("✔ TextMeshPro resources found.");

            if (ThingWorldPaths.FindXROriginPrefab() == null)
                report.AppendLine("✖ 'XR Origin (XR Rig)' not found → Package Manager → XR Interaction Toolkit → Samples → import Starter Assets.");
            else report.AppendLine("✔ XR Origin (XR Rig) prefab found.");

            report.AppendLine();
            report.AppendLine("Also check by hand (see the setup guide, Parts 3 and 7):");
            report.AppendLine("• Build profile is Meta Quest, OpenXR + Meta Quest Support enabled");
            report.AppendLine("• XR Interaction Toolkit → 'Use XR Interaction Simulator in scenes' is on");

            AssetDatabase.SaveAssets();
            Selection.activeObject = config;
            EditorUtility.DisplayDialog("Thing World — Set Up Project", report.ToString(), "OK");
        }

        static Material CreateSparkMaterial()
        {
            var existing = AssetDatabase.LoadAssetAtPath<Material>(ThingWorldPaths.SparkMaterialPath);
            if (existing != null) return existing;

            Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
            if (shader == null)
            {
                Debug.LogError("[Thing World] URP particle shader not found. Is this a URP (Universal 3D) project?");
                return null;
            }
            var mat = new Material(shader) { name = "ThingSpark" };
            Texture2D dot = AssetDatabase.GetBuiltinExtraResource<Texture2D>("Default-Particle.psd");
            if (dot != null) mat.SetTexture("_BaseMap", dot);

            // Transparent, additive (glowy) particles.
            mat.SetFloat("_Surface", 1f);
            mat.SetFloat("_Blend", 2f);
            mat.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
            mat.SetFloat("_DstBlend", (float)BlendMode.One);
            mat.SetFloat("_SrcBlendAlpha", (float)BlendMode.One);
            mat.SetFloat("_DstBlendAlpha", (float)BlendMode.One);
            mat.SetFloat("_ZWrite", 0f);
            mat.SetOverrideTag("RenderType", "Transparent");
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.renderQueue = (int)RenderQueue.Transparent;

            AssetDatabase.CreateAsset(mat, ThingWorldPaths.SparkMaterialPath);
            return mat;
        }

        static AnimationClip Placeholder(string name)
        {
            string path = $"{ThingWorldPaths.Placeholders}/{name}.anim";
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (clip != null) return clip;
            clip = new AnimationClip { name = name };
            AssetDatabase.CreateAsset(clip, path);
            return clip;
        }

        /// <summary>
        /// The shared animator every character uses. Students never edit it: they fill the slots
        /// (Idle, Walk, Grabbed, Action1-3) in their own Animation Override Controller —
        /// first with Mixamo clips, later with their own motion capture.
        /// </summary>
        static AnimatorController CreateBaseAnimator()
        {
            var existing = AssetDatabase.LoadAssetAtPath<AnimatorController>(ThingWorldPaths.AnimatorPath);
            if (existing != null) return existing;

            var clips = new Dictionary<string, AnimationClip>();
            foreach (string slot in ThingWorldPaths.AnimationSlots) clips[slot] = Placeholder(slot);

            AnimatorController c = AnimatorController.CreateAnimatorControllerAtPath(ThingWorldPaths.AnimatorPath);
            c.AddParameter("Speed", AnimatorControllerParameterType.Float);
            c.AddParameter("Grabbed", AnimatorControllerParameterType.Bool);
            c.AddParameter("Action1", AnimatorControllerParameterType.Trigger);
            c.AddParameter("Action2", AnimatorControllerParameterType.Trigger);
            c.AddParameter("Action3", AnimatorControllerParameterType.Trigger);

            AnimatorStateMachine sm = c.layers[0].stateMachine;

            AnimatorState locomotion = c.CreateBlendTreeInController("Locomotion", out BlendTree tree, 0);
            tree.blendType = BlendTreeType.Simple1D;
            tree.blendParameter = "Speed";
            tree.useAutomaticThresholds = false;
            tree.AddChild(clips["Idle"], 0f);
            tree.AddChild(clips["Walk"], 1.2f);
            sm.defaultState = locomotion;

            AnimatorState grabbed = sm.AddState("Grabbed");
            grabbed.motion = clips["Grabbed"];
            AnimatorStateTransition toGrabbed = sm.AddAnyStateTransition(grabbed);
            toGrabbed.AddCondition(AnimatorConditionMode.If, 0f, "Grabbed");
            toGrabbed.canTransitionToSelf = false;
            toGrabbed.duration = 0.1f;
            AnimatorStateTransition fromGrabbed = grabbed.AddTransition(locomotion);
            fromGrabbed.AddCondition(AnimatorConditionMode.IfNot, 0f, "Grabbed");
            fromGrabbed.hasExitTime = false;
            fromGrabbed.duration = 0.25f;

            for (int i = 1; i <= 3; i++)
            {
                AnimatorState action = sm.AddState("Action" + i);
                action.motion = clips["Action" + i];
                AnimatorStateTransition into = sm.AddAnyStateTransition(action);
                into.AddCondition(AnimatorConditionMode.If, 0f, "Action" + i);
                into.canTransitionToSelf = false;
                into.duration = 0.15f;
                AnimatorStateTransition back = action.AddTransition(locomotion);
                back.hasExitTime = true;
                back.exitTime = 0.9f;
                back.duration = 0.2f;
                AnimatorStateTransition interrupt = action.AddTransition(grabbed);
                interrupt.AddCondition(AnimatorConditionMode.If, 0f, "Grabbed");
                interrupt.hasExitTime = false;
                interrupt.duration = 0.1f;
            }

            EditorUtility.SetDirty(c);
            return c;
        }

        static ThingWorldConfig CreateOrUpdateConfig(Material spark, RuntimeAnimatorController animator)
        {
            var config = AssetDatabase.LoadAssetAtPath<ThingWorldConfig>(ThingWorldPaths.ConfigPath);
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<ThingWorldConfig>();
                AssetDatabase.CreateAsset(config, ThingWorldPaths.ConfigPath);
            }
            if (spark != null) config.sparkMaterial = spark;
            if (animator != null) config.baseAnimator = animator;
            EditorUtility.SetDirty(config);
            return config;
        }

        /// <summary>A capsule-shaped test neighbor, so students have someone to meet in their test scene.</summary>
        static void CreateDummyPrefab()
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(ThingWorldPaths.DummyPrefabPath) != null) return;

            var root = new GameObject("ExampleBlob");
            GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Object.DestroyImmediate(shape.GetComponent<Collider>());
            shape.name = "Body";
            shape.transform.SetParent(root.transform, false);
            shape.transform.localPosition = new Vector3(0f, 0.6f, 0f);
            shape.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            ConfigureThingRoot(root, new Bounds(new Vector3(0f, 0.6f, 0f), new Vector3(0.6f, 1.2f, 0.6f)));
            root.AddComponent<ExampleBlob>();

            PrefabUtility.SaveAsPrefabAsset(root, ThingWorldPaths.DummyPrefabPath);
            Object.DestroyImmediate(root);
        }

        /// <summary>Adds and configures every component a Thing needs (except the Thing script itself).</summary>
        public static void ConfigureThingRoot(GameObject root, Bounds localBounds)
        {
            var capsule = GetOrAdd<CapsuleCollider>(root);
            float height = Mathf.Max(0.2f, localBounds.max.y);
            float radius = Mathf.Clamp(Mathf.Max(localBounds.extents.x, localBounds.extents.z) * 0.8f, 0.1f, height * 0.5f);
            capsule.height = height;
            capsule.radius = radius;
            capsule.center = new Vector3(localBounds.center.x, height * 0.5f, localBounds.center.z);

            var body = GetOrAdd<Rigidbody>(root);
            body.isKinematic = true;
            body.interpolation = RigidbodyInterpolation.Interpolate;

            var agent = GetOrAdd<NavMeshAgent>(root);
            agent.radius = radius;
            agent.height = height;
            agent.enabled = false;

            var grab = GetOrAdd<XRGrabInteractable>(root);
            grab.movementType = XRBaseInteractable.MovementType.Instantaneous;
            grab.throwOnDetach = true;
            grab.useDynamicAttach = true;

            var audio = GetOrAdd<AudioSource>(root);
            audio.playOnAwake = false;
            audio.spatialBlend = 1f;

            var modifier = GetOrAdd<NavMeshModifier>(root);
            modifier.ignoreFromBuild = true;
        }

        static T GetOrAdd<T>(GameObject go) where T : Component
        {
            T c = go.GetComponent<T>();
            return c != null ? c : go.AddComponent<T>();
        }

        static bool EnsureTeleportLayer()
        {
            if (LayerMask.LayerToName(31) == "Teleport") return true;
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if (assets == null || assets.Length == 0) return false;
            var tagManager = new SerializedObject(assets[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");
            if (layers == null || layers.arraySize <= 31) return false;
            SerializedProperty l31 = layers.GetArrayElementAtIndex(31);
            if (!string.IsNullOrEmpty(l31.stringValue) && l31.stringValue != "Teleport")
            {
                Debug.LogWarning($"[Thing World] Layer 31 is already used as '{l31.stringValue}'. The Starter Assets expect 'Teleport'.");
                return false;
            }
            l31.stringValue = "Teleport";
            tagManager.ApplyModifiedProperties();
            return true;
        }

        // ------------------------------------------------------------------
        //  SCENES
        // ------------------------------------------------------------------

        /// <summary>Builds a fresh scene with sun, sky, walkable ground, the player rig and a ThingWorldManager.</summary>
        public static Scene BuildScene(string path, float groundSize, bool withDummies, float secondsPerDay, float spawnRadius)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Sun + day/night
            var sunGo = new GameObject("Sun (Day Night Cycle)");
            var sun = sunGo.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.shadows = LightShadows.Soft;
            var cycle = sunGo.AddComponent<DayNightCycle>();
            cycle.secondsPerDay = secondsPerDay;
            RenderSettings.sun = sun;
            Material sky = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Skybox.mat");
            if (sky != null)
            {
                RenderSettings.skybox = sky;
                RenderSettings.ambientMode = AmbientMode.Skybox;
            }

            // Ground
            if (groundSize > 0f)
            {
                GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
                ground.name = "Ground (placeholder)";
                ground.transform.localScale = new Vector3(groundSize / 10f, 1f, groundSize / 10f);
                GameObjectUtility.SetStaticEditorFlags(ground, StaticEditorFlags.BatchingStatic);
            }

            // Walkable surface (baked when the scene starts, or by the instructor for the city)
            var navGo = new GameObject("Walkable Ground (NavMeshSurface)");
            var surface = navGo.AddComponent<NavMeshSurface>();
            surface.collectObjects = CollectObjects.All;
            surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;

            // Manager
            var managerGo = new GameObject("Thing World Manager");
            var manager = managerGo.AddComponent<ThingWorldManager>();
            manager.navMeshSurface = surface;
            manager.spawnRadius = spawnRadius;
            if (withDummies)
            {
                var dummy = AssetDatabase.LoadAssetAtPath<GameObject>(ThingWorldPaths.DummyPrefabPath);
                if (dummy != null) for (int i = 0; i < 3; i++) manager.extraThings.Add(dummy);
            }

            // Player
            GameObject rigPrefab = ThingWorldPaths.FindXROriginPrefab();
            if (rigPrefab != null)
            {
                var rig = (GameObject)PrefabUtility.InstantiatePrefab(rigPrefab, scene);
                rig.transform.position = new Vector3(0f, 0f, -3f);
            }
            else
            {
                var cam = new GameObject("Main Camera (no XR rig found)");
                cam.tag = "MainCamera";
                cam.AddComponent<Camera>();
                cam.AddComponent<AudioListener>();
                cam.transform.position = new Vector3(0f, 1.6f, -3f);
                Debug.LogWarning("[Thing World] 'XR Origin (XR Rig)' not found. Import the XR Interaction Toolkit Starter Assets sample.");
            }

            ThingWorldPaths.EnsureFolder(Path.GetDirectoryName(path).Replace('\\', '/'));
            EditorSceneManager.SaveScene(scene, path);
            return scene;
        }

        [MenuItem("Thing World/Instructor/Create City Scene", priority = 210)]
        public static void CreateCityScene()
        {
            if (File.Exists(ThingWorldPaths.WorldScenePath) &&
                !EditorUtility.DisplayDialog("Thing World", "The city scene already exists. Replace it with an empty one?", "Replace", "Cancel"))
                return;
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            BuildScene(ThingWorldPaths.WorldScenePath, groundSize: 100f, withDummies: false, secondsPerDay: 300f, spawnRadius: 30f);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ThingWorldPaths.WorldScenePath, true) };
            EditorUtility.DisplayDialog("Thing World",
                "City scene created and set as the scene that builds.\n\n" +
                "1. Import everyone's .unitypackage files.\n" +
                "2. Drag each student's pieces (Assets/Creators/NAME/Pieces) into the scene and arrange the city together.\n" +
                "3. Thing World → Instructor → Bake City NavMesh.\n" +
                "4. Press Play — every student's Thing appears automatically.", "OK");
        }

        [MenuItem("Thing World/Instructor/Bake City NavMesh", priority = 211)]
        public static void BakeCityNavMesh()
        {
            var surface = Object.FindFirstObjectByType<NavMeshSurface>();
            if (surface == null)
            {
                EditorUtility.DisplayDialog("Thing World", "No NavMeshSurface in the open scene. Open the city scene first.", "OK");
                return;
            }

            foreach (var t in Object.FindObjectsByType<Thing>(FindObjectsSortMode.None))
                GetOrAdd<NavMeshModifier>(t.gameObject).ignoreFromBuild = true;
            foreach (var c in Object.FindObjectsByType<CharacterController>(FindObjectsSortMode.None))
                GetOrAdd<NavMeshModifier>(c.gameObject).ignoreFromBuild = true;

            surface.BuildNavMesh();
            if (surface.navMeshData == null)
            {
                EditorUtility.DisplayDialog("Thing World", "Nothing walkable was found. Do the city pieces have colliders?", "OK");
                return;
            }

            Scene scene = surface.gameObject.scene;
            string folder = string.IsNullOrEmpty(scene.path) ? ThingWorldPaths.WorldFolder : Path.GetDirectoryName(scene.path).Replace('\\', '/');
            string assetPath = $"{folder}/{scene.name}_NavMesh.asset";
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.CreateAsset(surface.navMeshData, assetPath);
            EditorUtility.SetDirty(surface);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("[Thing World] NavMesh baked and saved: " + assetPath);
            EditorUtility.DisplayDialog("Thing World", "Walkable ground baked. Blue areas in the Scene view are where Things can walk.", "OK");
        }

        [MenuItem("Thing World/Help — Open the Handbook", priority = 300)]
        public static void OpenHandbook()
        {
            Application.OpenURL("https://github.com/WeitongSun/3D-Computer-Art/blob/main/thing-world/HANDBOOK.md");
        }
    }
}
