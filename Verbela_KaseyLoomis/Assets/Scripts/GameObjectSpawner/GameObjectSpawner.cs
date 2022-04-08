using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace verb
{
    public partial class GameObjectSpawner : MonoBehaviour
    {
        public GameObjectSpawnerData spawnData;

        public int amountToSpawn;

        //man i love this attribute. Had to use this, but it's not somthing you normally keep around. 
        //I wanted to leave the comment here in a show-my-work kind of way
        //[UnityEngine.Serialization.FormerlySerializedAs("parent")]
        public Transform spawnParent;

        public bool spawnOnEnable;
        
        public List<GameObject> DoSpawn()
        {
            return spawnData.Spawn(amountToSpawn, spawnParent);
        }

        public List<T> DoSpawn<T>()
            where T : Component
        {
            return spawnData.Spawn<T>(amountToSpawn, spawnParent);
        }

        protected virtual void OnEnable()
        {
            if (spawnOnEnable)
                DoSpawn();
        }
    }
}
