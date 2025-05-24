using UnityEngine;
using UnityEditor;

namespace Client.Editor
{
    public class MaterialFixerEditor : EditorWindow
    {
        private DefaultAsset targetFolder;

        [MenuItem("Tools/Convert Materials to URP")]
        public static void ShowWindow()
        {
            GetWindow<MaterialFixerEditor>("URP Material Converter");
        }

        private void OnGUI()
        {
            GUILayout.Label("Convert materials in folder to URP/Lit", EditorStyles.boldLabel);
            targetFolder =
                (DefaultAsset)EditorGUILayout.ObjectField("Target Folder", targetFolder, typeof(DefaultAsset), false);

            if (GUILayout.Button("Convert Materials"))
            {
                if (targetFolder == null)
                {
                    Debug.LogError("❌ Please assign a folder.");
                    return;
                }

                string folderPath = AssetDatabase.GetAssetPath(targetFolder);
                ConvertMaterialsInFolder(folderPath);
            }
        }

        private void ConvertMaterialsInFolder(string folderPath)
        {
            var guids = AssetDatabase.FindAssets("t:Material", new[] { folderPath });
            var convertedCount = 0;

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var mat = AssetDatabase.LoadAssetAtPath<Material>(path);

                if (mat == null) continue;

                var oldShader = mat.shader;
                Texture oldMainTex = null;

                if (mat.HasProperty("_BaseMap"))
                    oldMainTex = mat.GetTexture("_BaseMap");
                else if (mat.HasProperty("_MainTex"))
                    oldMainTex = mat.GetTexture("_MainTex");

                mat.shader = Shader.Find("Universal Render Pipeline/Lit");

                if (oldMainTex != null)
                    mat.SetTexture("_BaseMap", oldMainTex);

                EditorUtility.SetDirty(mat);
                convertedCount++;
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"✅ Converted {convertedCount} materials to URP Lit in folder: {folderPath}");
        }
    }
}