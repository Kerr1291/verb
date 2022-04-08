using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace verb
{
    public static class GameObjectSpawnerDataExtensions
    {
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

        public static List<T> Spawn<T>(this GameObjectSpawnerData data, int count, Transform parent = null)
            where T : Component
        {
            return data.Spawn(count, parent).Select(x => x.GetComponent<T>()).ToList();
        }
    }
}
