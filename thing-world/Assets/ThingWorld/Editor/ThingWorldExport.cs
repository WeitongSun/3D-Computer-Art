using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ThingWorldEditor
{
    /// <summary>
    /// "Thing World → 4. Export My Work" — packs Assets/Creators/YourName into one .unitypackage on your Desktop.
    /// "Thing World → 5. Make City Piece From Selected Model" — turns a model into a city piece prefab.
    /// </summary>
    public static class ThingWorldExport
    {
        [MenuItem("Thing World/4. Export My Work", priority = 21)]
        static void Export()
        {
            ThingWorldPaths.WithCreator("Export My Work", name =>
            {
                var results = ThingWorldChecker.Check(name);
                int errors = results.Count(r => r.level == ThingWorldChecker.Level.Error);
                if (errors > 0)
                {
                    int choice = EditorUtility.DisplayDialogComplex("Thing World",
                        $"'Check My Work' found {errors} problem(s). Please fix them first — otherwise your work may break the class city.",
                        "Show Problems", "Cancel", "Export Anyway");
                    if (choice == 0) { EditorApplication.ExecuteMenuItem("Thing World/3. Check My Work"); return; }
                    if (choice == 1) return;
                }

                AssetDatabase.SaveAssets();
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string file = Path.Combine(desktop, $"{name}_ThingWorld_{DateTime.Now:yyyy-MM-dd_HHmm}.unitypackage");
                AssetDatabase.ExportPackage(ThingWorldPaths.CreatorFolder(name), file, ExportPackageOptions.Recurse);
                EditorUtility.RevealInFinder(file);
                EditorUtility.DisplayDialog("Thing World",
                    $"Exported to your Desktop:\n{Path.GetFileName(file)}\n\nHand this one file in. It contains your whole folder.", "OK");
            });
        }

        [MenuItem("Thing World/5. Make City Piece From Selected Model", priority = 40)]
        static void MakePiece()
        {
            var model = Selection.activeObject as GameObject;
            string path = model != null ? AssetDatabase.GetAssetPath(model) : null;
            if (string.IsNullOrEmpty(path))
            {
                EditorUtility.DisplayDialog("Thing World", "First click your model (FBX) in the Project window, then use this menu.", "OK");
                return;
            }

            string name = ThingWorldPaths.AllCreators().FirstOrDefault(n => ThingWorldPaths.IsInside(path, ThingWorldPaths.CreatorFolder(n)));
            if (name == null)
            {
                EditorUtility.DisplayDialog("Thing World",
                    "Put the model inside your own folder first:\nAssets/Creators/YourName/Pieces", "OK");
                return;
            }

            ThingWorldPaths.EnsureCreatorFolders(name);
            string baseName = model.name.StartsWith(name + "_", StringComparison.Ordinal) ? model.name : $"{name}_{model.name}";
            string prefabPath = AssetDatabase.GenerateUniqueAssetPath($"{ThingWorldPaths.PiecesFolder(name)}/{baseName}.prefab");

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(model);
            PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            instance.name = Path.GetFileNameWithoutExtension(prefabPath);
            instance.transform.position = Vector3.zero;

            foreach (Camera c in instance.GetComponentsInChildren<Camera>(true)) UnityEngine.Object.DestroyImmediate(c.gameObject);
            foreach (Light l in instance.GetComponentsInChildren<Light>(true)) UnityEngine.Object.DestroyImmediate(l.gameObject);

            foreach (Transform t in instance.GetComponentsInChildren<Transform>(true))
            {
                GameObject g = t.gameObject;
                MeshFilter mf = g.GetComponent<MeshFilter>();
                if (mf != null && mf.sharedMesh != null && g.GetComponent<Collider>() == null)
                    g.AddComponent<MeshCollider>().sharedMesh = mf.sharedMesh;
                if (g.GetComponent<Renderer>() != null)
                    GameObjectUtility.SetStaticEditorFlags(g, StaticEditorFlags.BatchingStatic | StaticEditorFlags.ContributeGI |
                                                              StaticEditorFlags.OccluderStatic | StaticEditorFlags.OccludeeStatic);
            }

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
            UnityEngine.Object.DestroyImmediate(instance);
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);
            Debug.Log($"[Thing World] City piece created: {prefabPath}");
        }
    }
}
