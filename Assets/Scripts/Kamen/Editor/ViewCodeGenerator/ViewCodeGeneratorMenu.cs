/*
* @Author: Kamen
* @Description:
* @Date: 2023年11月06日 星期一 11:11:55
* @Modify:
*/
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace KamenGameFramewrok.Editor
{
    static class ViewCodeGeneratorMenu
    {
        [MenuItem("Assets/代码生成/New一键代码生成")]
        private static void GenerateUICodeNew()
        {
            UnityEngine.Object[] selObjs = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            if (selObjs == null || selObjs.Length == 0)
            {
                Debug.LogError("请选择prefab");
                return;
            }

            List<GameObject> selectedPrefabs = new List<GameObject>(selObjs.Length);
            HashSet<string> selectedAssetPathSet = new HashSet<string>();

            foreach (UnityEngine.Object obj in selObjs)
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);

                if (assetPath.EndsWith(".prefab") && obj is GameObject)
                {
                    selectedPrefabs.Add(obj as GameObject);
                }

                selectedAssetPathSet.Add(assetPath);
            }

            EditorUtility.DisplayProgressBar("生成代码", "", 0);
            string rootDir = Path.GetFullPath(Path.Combine(Application.dataPath, "../"));
            for (int i = 0; i < selectedPrefabs.Count; ++i)
            {
                EditorUtility.DisplayProgressBar("生成代码", "", (float) i / selectedPrefabs.Count);
                GameObject selectedPrefab = selectedPrefabs[i];
                string assetPath = AssetDatabase.GetAssetPath(selectedPrefab);
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                if (null != importer)
                {
                    ViewCodeGenerator.Generate(rootDir, $"{importer.assetBundleName}",
                        selectedPrefab, importer.assetPath);
                }
            }

            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }
}