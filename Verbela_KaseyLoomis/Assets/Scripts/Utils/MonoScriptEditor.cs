//A cutom helper script to make it easy to create scriptable objects during development.
//It's currently disabled to remove some obnoxious error spam that can happen due to a unity bug that's been around for a while.
//(It might be fixed in the newer versions of unity, but it's still around for now)
#define DELETE_ME_TO_ENABLE //just comment this line to enable the script
#if !DELETE_ME_TO_ENABLE
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
    /// <summary>
    /// This script parses the MonoScript files in the editor and when it locates one that derives from scriptable object
    /// it adds a button to editor that allows the instantiation of a scriptable object unity asset. I find it removes a 
    /// lot of boilerplate when working with smaller scriptable objects and helps keep my code cleaner.
    /// </summary>
    [CustomEditor(typeof(MonoScript))]
    public class MonoScriptEditor : AssetImporterEditor
    {
        /// <summary>
        /// Because overriding the default editor causes unity to stop showing file previews, I put some code in to add that back.
        /// This makes it show up to 2000 lines of text from the scripts in a preview window, like the original.
        /// </summary>
        const int numberOfLinesInPreview = 2000;

        /// <summary>
        /// Implement the custom editor adding a button to make scriptable objects and re-adding a preview window.
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (target == null)
                return;

            Type t = null;

            //check all loaded assemblies to see if the type within the script is a scriptable object
            //this does use the name of the script, but since unity requires that unity objects have the file name match the class
            //name within, this allows us to take advantage of that as well.
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                t = asm.GetTypes().Where(x => x.Name.Contains(target.name)).Where(x => x.IsSubclassOf(typeof(ScriptableObject))).FirstOrDefault();
                if (t != null)
                    break;
            }

            //name of script
            EditorGUILayout.LabelField("Script: " + target.name);

            //make a button to create a scriptable object
            if (t != null && t.IsSubclassOf(typeof(ScriptableObject)) && !t.IsAbstract && !t.IsGenericType)
            {
                if (GUILayout.Button("Create Scriptable Object", GUILayout.MaxWidth(320)))
                {
                    //just some code to give us a nice file path to start with to make things easier for the user
                    string startingAssetLocation = $"{Application.dataPath}";
                    string[] foundAssets = AssetDatabase.FindAssets(target.name);
                    if (foundAssets.Length > 0)
                    {
                        startingAssetLocation = System.IO.Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(foundAssets[0]));
                    }

                    //actually make the asset
                    CreateScriptableObject(t, startingAssetLocation);
                }
            }

            //by overwriting their default script inspector we'll lose the preview text, so let's add it back over here
            string sourceText = ((MonoScript)target).text;
            string previewText = sourceText.Substring(0, Mathf.Min(sourceText.Length, numberOfLinesInPreview))
                + (sourceText.Length > 2000 ? "\n <Preview truncated>" : "");

            EditorGUILayout.TextArea(previewText);
            
            base.OnInspectorGUI();
        }

        /// <summary>
        /// Not needed for this demo, but I provided this as an example of what I'd normally do in this circumstance
        /// since these type constraints are just so wonderful (really i think they're great).
        /// </summary>
        /// <typeparam name="TDataObject">ScriptableObject derived type to be instantiated</typeparam>
        /// <param name="path">The path to place the created asset</param>
        /// <returns>The newly instantiated asset</returns>
        public static ScriptableObject CreateScriptableObject<TDataObject>(string path)
            where TDataObject : ScriptableObject
        {
            return CreateScriptableObject(typeof(TDataObject), path);
        }

        /// <summary>
        /// Creates and saves an asset derived from ScriptableObject by providing a file browser panel for the user to choose the location of where to instantate the asset.
        /// </summary>
        /// <param name="type">ScriptableObject derived type to be instantiated</param>
        /// <param name="path">The path to place the created asset</param>
        /// <returns>The newly instantiated asset</returns>
        public static ScriptableObject CreateScriptableObject(Type type, string path)
        {
            var foundAssets = AssetDatabase.FindAssets(type.Name);
            string countString = foundAssets.Length.ToString().PadLeft(3, '0');

            path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", "" + type.Name + "_" + countString + ".asset", "asset", "Enter a name for the " + type.Name + ".", path);
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
#endif