#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
namespace verb
{
    [CustomEditor(typeof(GameObjectSpawner))]
    public class GameObjectSpawnerEditor : Editor
    {
        GameObjectSpawner Target {
            get {
                return ((GameObjectSpawner)target);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            if (GUILayout.Button("Spawn objects", GUILayout.MaxWidth(200)))
            {
                var spawnedObjects = Target.DoSpawn();

                spawnedObjects.ForEach(x => Undo.RegisterCreatedObjectUndo(x, "Spawned entities"));
            }
        }
    }

    public partial class GameObjectSpawner : MonoBehaviour
    {
        protected virtual void OnDrawGizmosSelected()
        {
            if (spawnData != null && spawnData.objToSpawn != null)
            {
                Transform parent = spawnParent;
                Vector3 origin = parent == null ? transform.position : parent.transform.position;

                Color savedColor = Handles.color;

                Handles.Label(origin + Vector3.right * spawnData.spawnRadius, spawnData.objToSpawn.name + " spawn area");
                Handles.color = spawnData.spawnRadiusColor;

                if (spawnData.spawnIs2D)
                {
                    Handles.DrawWireDisc(origin, Vector3.up, spawnData.spawnRadius);
                }
                else
                {
                    Handles.DrawWireDisc(origin, Vector3.up, spawnData.spawnRadius);
                    Handles.DrawWireDisc(origin, Vector3.right, spawnData.spawnRadius);
                    Handles.DrawWireDisc(origin, Vector3.forward, spawnData.spawnRadius);
                }

                Handles.color = savedColor;
            }
        }
    }
}
#endif