#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System;
using System.Reflection;
using System.Linq;

namespace verb.editor
{
    [CustomEditor(typeof(MonoScript))]
    public class MonoScriptEditor : AssetImporterEditor
    {
        const int numberOfZeroesToPad = 3;
        const int numberOfLinesInPreview = 2000;

        public override void OnInspectorGUI()
        {
            if (target == null)
                return;

            Type t = null;

            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                t = asm.GetTypes().Where(x => x.Name.Contains(target.name)).Where(x => x.IsSubclassOf(typeof(ScriptableObject))).FirstOrDefault();
                if (t != null)
                    break;
            }


            EditorGUILayout.LabelField("Script: " + target.name);
            if (t != null && t.IsSubclassOf(typeof(ScriptableObject)) && !t.IsAbstract && !t.IsGenericType)
            {
                if (GUILayout.Button("Create Scriptable Object", GUILayout.MaxWidth(320)))
                {
                    string startingAssetLocation = $"{Application.dataPath}";
                    string[] foundAssets = AssetDatabase.FindAssets(target.name);
                    if (foundAssets.Length > 0)
                    {
                        startingAssetLocation = System.IO.Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(foundAssets[0]));
                    }

                    CreateAssetWithSavePrompt(t, startingAssetLocation);
                }
            }

            //by overwriting their default script inspector we'll lose the preview text, so let's add it back over here
            string sourceText = ((MonoScript)target).text;
            string previewText = sourceText.Substring(0, Mathf.Min(sourceText.Length, numberOfLinesInPreview))
                + (sourceText.Length > 2000 ? "\n <Preview truncated>" : "");

            EditorGUILayout.TextArea(previewText);


            base.OnInspectorGUI();
        }


        public static ScriptableObject CreateAssetWithSavePrompt<TDataObject>(string path)
            where TDataObject : ScriptableObject
        {
            return CreateAssetWithSavePrompt(typeof(TDataObject), path);
        }

        // Creates a new ScriptableObject via the default Save File panel
        public static ScriptableObject CreateAssetWithSavePrompt(Type type, string path)
        {
            var foundAssets = AssetDatabase.FindAssets(type.Name);
            string count_as_int = foundAssets.Length.ToString().PadLeft(numberOfZeroesToPad, '0');

            path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", "" + type.Name + "_" + count_as_int + ".asset", "asset", "Enter a name for the " + type.Name + ".", path);
            if (path == "")
                return null;

            ScriptableObject asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            EditorGUIUtility.PingObject(asset);
            return asset;
        }
    }
}
#endif