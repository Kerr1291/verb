using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace verb
{
    /// <summary>
    /// Unity component that allows for scene interactions with the GameObjectSpawner data.
    /// </summary>
    public partial class GameObjectSpawner : MonoBehaviour
    {
        [Header("Parameters to configure the thing that should be spawned")]
        /// <summary>
        /// The data source 
        /// </summary>
        public GameObjectSpawnerData spawnData;

        [Space]
        [Header("Parameters to configure how the objects should spawn")]
        /// <summary>
        /// How many objects should spawn when this behaviour is activated.
        /// </summary>
        public int amountToSpawn;

        //i really really like this attribute. Had to use this, but it's not somthing you normally keep around. 
        //I wanted to leave the comment here in a show-my-work kind of way, but also because more people should know of this thing
        //[UnityEngine.Serialization.FormerlySerializedAs("parent")]

        /// <summary>
        /// Optional parent to parent the newly spawned objects to.
        /// </summary>
        public Transform spawnParent;

        /// <summary>
        /// Should this behvaiour activate when the game object is turned on?
        /// </summary>
        public bool spawnOnEnable;

        [Space]
        [Header("Hotkey for spawning objects")]
        /// <summary>
        /// Hotkey that may be used to activate the spawn behaviour by the user.
        /// </summary>
        public KeyCode spawn1HotKey = KeyCode.A;
        
        /// <summary>
        /// Use the parameters contained in this object to spawn some game objects.
        /// </summary>
        /// <returns>Returns a list of the spawned objects.</returns>
        public List<GameObject> DoSpawn()
        {
            return spawnData.Spawn(amountToSpawn, spawnParent);
        }

        /// <summary>
        /// Use the parameters contained in this object to spawn some game objects.
        /// </summary>
        /// <typeparam name="T">The type of component on the spawened objects to return a reference to</typeparam>
        /// <returns>Returns a list of references to components on the spawned objects.</returns>
        public List<T> DoSpawn<T>()
            where T : Component
        {
            return spawnData.Spawn<T>(amountToSpawn, spawnParent);
        }

        /// <summary>
        /// Activates when a unity game object is turned on. (every time it's turned on)
        /// </summary>
        protected virtual void OnEnable()
        {
            if (spawnOnEnable)
                DoSpawn();
        }

        /// <summary>
        /// Check the key down state every frame
        /// </summary>
        protected virtual void Update()
        {
            if(Input.GetKeyDown(spawn1HotKey))
            {
                DoSpawn();
            }
        }
    }
}
