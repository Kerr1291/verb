using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace verb
{
    public class GameObjectSpawnerData : ScriptableObject
    {
        public GameObject objToSpawn;

        public float spawnRadius;
        public bool spawnIs2D;

        public Color spawnRadiusColor = Color.cyan;
    }
}
