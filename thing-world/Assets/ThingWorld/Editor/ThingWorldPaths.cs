using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ThingWorldEditor
{
    /// <summary>Folder layout and small helpers shared by all Thing World editor tools.</summary>
    public static class ThingWorldPaths
    {
        public const string Root = "Assets/ThingWorld";
        public const string Generated = Root + "/Generated";
        public const string Placeholders = Generated + "/AnimationPlaceholders";
        public const string ConfigFolder = Root + "/Resources";
        public const string ConfigPath = ConfigFolder + "/ThingWorldConfig.asset";
        public const string SparkMaterialPath = Generated + "/ThingSpark.mat";
        public const string AnimatorPath = Generated + "/ThingAnimator.controller";
        public const string DummyPrefabPath = Generated + "/ExampleBlob.prefab";
        public const string CreatorsRoot = "Assets/Creators";
        public const string WorldFolder = "Assets/World";
        public const string WorldScenePath = WorldFolder + "/ThingWorld_City.unity";

        public static readonly string[] AnimationSlots = { "Idle", "Walk", "Grabbed", "Action1", "Action2", "Action3" };

        public static string CreatorFolder(string name) => $"{CreatorsRoot}/{name}";
        public static string CharacterFolder(string name) => $"{CreatorFolder(name)}/Character";
        public static string ScriptsFolder(string name) => $"{CreatorFolder(name)}/Scripts";
        public static string ThingsFolder(string name) => $"{CreatorFolder(name)}/Resources/Things";
        public static string AnimationFolder(string name) => $"{CreatorFolder(name)}/Animation";
        public static string PiecesFolder(string name) => $"{CreatorFolder(name)}/Pieces";
        public static string SoundsFolder(string name) => $"{CreatorFolder(name)}/Sounds";
        public static string ClassName(string name) => $"{name}Thing";
        public static string ScriptPath(string name) => $"{ScriptsFolder(name)}/{ClassName(name)}.cs";
        public static string PrefabPath(string name) => $"{ThingsFolder(name)}/{ClassName(name)}.prefab";
        public static string OverridePath(string name) => $"{AnimationFolder(name)}/{name}_Animations.overrideController";
        public static string TestScenePath(string name) => $"{CreatorFolder(name)}/{name}_TestScene.unity";

        /// <summary>Letters and numbers only, starting with a letter. Returns the cleaned name or null.</summary>
        public static string CleanName(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            string n = Regex.Replace(raw.Trim(), "[^A-Za-z0-9]", "");
            if (n.Length == 0 || !char.IsLetter(n[0])) return null;
            return char.ToUpperInvariant(n[0]) + n.Substring(1);
        }

        /// <summary>Names of all student folders in Assets/Creators.</summary>
        public static List<string> AllCreators()
        {
            if (!AssetDatabase.IsValidFolder(CreatorsRoot)) return new List<string>();
            return AssetDatabase.GetSubFolders(CreatorsRoot)
                .Select(Path.GetFileName)
                .Where(n => !n.StartsWith("_"))
                .OrderBy(n => n)
                .ToList();
        }

        /// <summary>
        /// Figure out whose project this is. One folder in Assets/Creators → that name.
        /// Several → ask. None → tell them to create their Thing first.
        /// </summary>
        public static void WithCreator(string actionTitle, Action<string> then)
        {
            List<string> all = AllCreators();
            if (all.Count == 0)
            {
                EditorUtility.DisplayDialog("Thing World",
                    "There is no student folder yet.\n\nStart with 'Thing World → 1. Create or Update My Thing'.", "OK");
                return;
            }
            if (all.Count == 1) { then(all[0]); return; }
            PickCreatorWindow.Open(actionTitle, all, then);
        }

        public static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            string parent = Path.GetDirectoryName(path).Replace('\\', '/');
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, Path.GetFileName(path));
        }

        public static void EnsureCreatorFolders(string name)
        {
            EnsureFolder(CharacterFolder(name));
            EnsureFolder(ScriptsFolder(name));
            EnsureFolder(ThingsFolder(name));
            EnsureFolder(AnimationFolder(name));
            EnsureFolder(PiecesFolder(name));
            EnsureFolder(SoundsFolder(name));
        }

        public static string FindTemplatePath()
        {
            foreach (string guid in AssetDatabase.FindAssets("ThingTemplate.cs"))
            {
                string p = AssetDatabase.GUIDToAssetPath(guid);
                if (p.EndsWith("ThingTemplate.cs.txt")) return p;
            }
            return null;
        }

        /// <summary>The XR Origin prefab from the XR Interaction Toolkit Starter Assets sample.</summary>
        public static GameObject FindXROriginPrefab()
        {
            foreach (string guid in AssetDatabase.FindAssets("\"XR Origin (XR Rig)\" t:Prefab"))
            {
                string p = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileNameWithoutExtension(p) == "XR Origin (XR Rig)")
                    return AssetDatabase.LoadAssetAtPath<GameObject>(p);
            }
            return null;
        }

        /// <summary>Find a compiled Thing class by name.</summary>
        public static Type FindThingType(string className)
        {
            foreach (Type t in TypeCache.GetTypesDerivedFrom<Thing>())
                if (t.Name == className && !t.IsAbstract) return t;
            return null;
        }

        public static bool IsInside(string assetPath, string folder) =>
            assetPath == folder || assetPath.StartsWith(folder + "/", StringComparison.Ordinal);
    }

    /// <summary>Tiny window: "Who are you?" when a project contains several student folders.</summary>
    public class PickCreatorWindow : EditorWindow
    {
        List<string> names;
        Action<string> callback;

        public static void Open(string title, List<string> names, Action<string> callback)
        {
            var w = CreateInstance<PickCreatorWindow>();
            w.titleContent = new GUIContent(title);
            w.names = names;
            w.callback = callback;
            w.minSize = new Vector2(280, 60 + names.Count * 24);
            w.ShowUtility();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Whose work is this?", EditorStyles.boldLabel);
            foreach (string n in names)
            {
                if (GUILayout.Button(n))
                {
                    Action<string> cb = callback;
                    Close();
                    cb?.Invoke(n);
                    GUIUtility.ExitGUI();
                }
            }
        }
    }
}
