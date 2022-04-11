using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace verb
{
    /// <summary>
    /// Methods to interact with GameObjectSpawner data.
    /// </summary>
    public static class GameObjectSpawnerDataExtensions
    {
        /// <summary>
        /// Spawn a number of game objects optionally parented to the given transform
        /// </summary>
        /// <param name="data">Data used to configure the spawn parameters</param>
        /// <param name="count">Number of objects to spawn</param>
        /// <param name="parent">OPTIONAL: Transform to parent to the spawned object</param>
        /// <returns>A list of references to each of the spawned objects</returns>
        public static List<GameObject> Spawn(this GameObjectSpawnerData data, int count, Transform parent = null)
        {
            List<GameObject> spawnedObjects = new List<GameObject>();
            for (int i = 0; i < count; ++i)
            {
                Vector3 spawnPosition = data.spawnRadius * (data.spawnIs2D ? UnityEngine.Random.insideUnitCircle.ToVector3XZ(0f) : UnityEngine.Random.insideUnitSphere);

                if (Application.isPlaying)
                {
                    spawnedObjects.Add(GameObject.Instantiate(data.objToSpawn, spawnPosition, Quaternion.identity, parent));
                }
                else
                {
#if UNITY_EDITOR
                    GameObject spawnedObject = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(data.objToSpawn, parent);
                    spawnedObject.transform.localPosition = spawnPosition;
                    spawnedObjects.Add(spawnedObject);
#endif
                }

            }

            return spawnedObjects;
        }

        /// <summary>
        /// Spawn a number of game objects optionally parented to the given transform
        /// </summary>
        /// <typeparam name="T">Type of component to get off the spawned objects</typeparam>
        /// <param name="data">Data used to configure the spawn parameters</param>
        /// <param name="count">Number of objects to spawn</param>
        /// <param name="parent">OPTIONAL: Transform to parent to the spawned object</param>
        /// <returns>A list of references to the given component type on each spawned object</returns>
        public static List<T> Spawn<T>(this GameObjectSpawnerData data, int count, Transform parent = null)
            where T : Component
        {
            return data.Spawn(count, parent).Select(x => x.GetComponent<T>()).ToList();
        }
    }
}
