using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace verb
{
    /// <summary>
    /// Data object that contains information that may be used to spawn a game object.
    /// </summary>
    public class GameObjectSpawnerData : ScriptableObject
    {
        [Header("GameObject or Prefab to spawn")]
        /// <summary>
        /// Reference to a GameObject/Prefab that may be created from this data.
        /// </summary>
        public GameObject objToSpawn;

        [Header("Size of random spawn area")]
        /// <summary>
        /// Used to determine an area for creation.
        /// </summary>
        public float spawnRadius;

        [Header("Used to specifiy if the creation area should be 2D (XZ) or 3D")]
        /// <summary>
        /// Used to specifiy if the creation area should be 2D (XZ) or 3D
        /// </summary>
        public bool spawnIs2D;

        [Header("Editor: Color of the spawn area")]
        /// <summary>
        /// The color of the editor gizmo used to display the spawn area.
        /// </summary>
        public Color spawnRadiusColor = Color.cyan;
    }
}
