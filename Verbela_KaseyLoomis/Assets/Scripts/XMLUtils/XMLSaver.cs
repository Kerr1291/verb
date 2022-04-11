using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;


namespace verb
{
    /// <summary>
    /// Saves/Loads objects to and from an XML file
    /// </summary>
    public class XMLSaver : MonoBehaviour
    {
        /// <summary>
        /// Just a quick note of instructions for testing
        /// </summary>
        [TextArea(5, 5)]
        public string note = "To test\nRight-Click this component -> \nSave Objects \nLoad Objects";

        /// <summary>
        /// Location to look for the file
        /// </summary>
        public string xmlDataFile = "XMLTest.xml";

        [System.Serializable]
        public class PrefabTypes
        {
            public string name;
            public GameObject prefab;
            public Transform parent;
            public string componentName;
            public string componentAssemblyName;
        }

        /// <summary>
        /// List of mappings for object names to types
        /// </summary>
        public List<PrefabTypes> prefabs;
        
        /// <summary>
        /// Get the PrefabTypes entry for the given name key
        /// </summary>
        public PrefabTypes GetPrefabData(string name)
        {
            return prefabs.FirstOrDefault(x => x.name.Equals(name));
        }

        /// <summary>
        /// Find loaded objects using the given component
        /// </summary>
        public List<Component> GetSceneObjects(Type componentType, bool includeInactive = true)
        {
            return GameObject.FindObjectsOfType(componentType, includeInactive).Cast<Component>().ToList();
        }

        /// <summary>
        /// Spawn objects in the scene
        /// </summary>
        public GameObject Spawn(GameObject prefab, Vector3 spawnPosition, Transform parent = null)
        {
            if (Application.isPlaying)
            {
                return GameObject.Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
            }
            else
            {
#if UNITY_EDITOR
                GameObject spawnedObject = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, parent);
                spawnedObject.transform.localPosition = spawnPosition;
                return spawnedObject;
#else
                return null;
#endif
            }
        }

        /// <summary>
        /// Extract the assembly name from the type name
        /// </summary>
        public void OnValidate()
        {
            foreach (var p in prefabs)
            {
                if (p.prefab != null && !string.IsNullOrEmpty(p.componentName))
                {
                    p.componentAssemblyName = Assembly.GetAssembly(p.prefab.GetComponent(p.componentName).GetType()).FullName;
                }
            }
        }

        /// <summary>
        /// Save the objects
        /// </summary>
        [ContextMenu("Save Objects")]
        public void Save()
        {
            XMLSceneData sceneData = new XMLSceneData();
            sceneData.sceneObjects =

            prefabs.Select(x =>
            {
                Assembly typeAssembly = Assembly.Load(x.componentAssemblyName);
                System.Type componentType = typeAssembly.GetType(x.componentName);

                //unity bug/ism can cause the get type to fail, brute force find it here
                if (componentType == null)
                {
                    foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        componentType = asm.GetTypes().Where(z => z.Name.Equals(x.componentName)).Where(q => q.IsSubclassOf(typeof(MonoBehaviour))).FirstOrDefault();
                        if (componentType != null)
                            break;
                    }
                }

                return new XMLObjectData()
                {
                    objectPositions = GetSceneObjects(componentType).Select(y => (y as MonoBehaviour).transform.localPosition).ToList(),
                    prefabName = x.name
                };
            }).ToList();

            XMLUtils.WriteDataToFile(Application.dataPath + "/" + xmlDataFile, sceneData);
        }

        /// <summary>
        /// Load the objects
        /// </summary>
        [ContextMenu("Load Objects")]
        public void Load()
        {
            XMLSceneData sceneData;
            XMLUtils.ReadDataFromFile(Application.dataPath + "/" + xmlDataFile, out sceneData);

            sceneData.sceneObjects.ForEach(x =>
                {
                    //grab template data
                    var prefabData = GetPrefabData(x.prefabName);

                    //spawn at each position
                    x.objectPositions.ForEach(y =>
                    {
                        Spawn(prefabData.prefab, y, prefabData.parent);
                    });

                });
        }
    }
}
