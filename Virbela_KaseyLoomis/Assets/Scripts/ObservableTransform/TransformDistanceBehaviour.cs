using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace verb
{
    /// <summary>
    /// Tracks the distance between this transform, the active transform, and the tracked transform. If this transform is closer to the tracked transform than the current active transform, then it becomes the new active transform.
    /// </summary>
    public class TransformDistanceBehaviour : MonoBehaviour
    {
        [Header("The primary transform that will be tracked")]
        /// <summary>
        /// Data object that allows subscribers to be notified of any changes to the moving transform.
        /// </summary>
        public ObservableTransform trackedTransform;

        [Header("The current closest object to the trackedTransform")]
        /// <summary>
        /// Data object that allows subscribers to be notified of any changes to the the currently active (highlighted) transform.
        /// </summary>
        public ObservableTransform activeTransform;

        [Header("Action(s) to perform when this object becomes the closest to the trackedTransform")]
        /// <summary>
        /// When a transform becomes active
        /// </summary>
        public UnityEngine.Events.UnityEvent onActive;

        [Header("Action(s) to perform when this object is not the closest to the trackedTransform")]
        /// <summary>
        /// When a transform becomes inactive
        /// </summary>
        public UnityEngine.Events.UnityEvent onInactive;
        
        /// <summary>
        /// When the game object is turned on, bind the listeners.
        /// </summary>
        public void OnEnable()
        {
            //subscribe
            trackedTransform.onObservedTransformChanged -= CheckClosest;
            trackedTransform.onObservedTransformChanged += CheckClosest;

            activeTransform.onObservedTransformChanged -= NotifyNewClosest;
            activeTransform.onObservedTransformChanged += NotifyNewClosest;

            if (trackedTransform.ObservedTransform != null)
            {
                //check when this object is enabled to see if it's now the closest
                CheckClosest(trackedTransform.ObservedTransform);
            }
        }

        /// <summary>
        /// When a game object is turned off, unbind
        /// </summary>
        public void OnDisable()
        {
            trackedTransform.onObservedTransformChanged -= CheckClosest;
            activeTransform.onObservedTransformChanged -= NotifyNewClosest;
            
            //force an update to re-calculate which transforms are closer if we're disabling the currently active transform
            if(activeTransform.ObservedTransform == transform)
                trackedTransform.Publish();
        }

        /// <summary>
        /// Check to see if this transform is closer than the active transform to the moving/tracked object
        /// </summary>
        /// <param name="other"></param>
        public void CheckClosest(Transform other)
        {
            //no highlighted transform, this one will claim that spot and trigger the NotifyNewCloest callback
            if (activeTransform.ObservedTransform == null)
            { 
                activeTransform.ObservedTransform = transform;
                return;
            }

            //is this the active one? then don't check its distance vs itself....
            if (activeTransform.ObservedTransform == transform)
                return;

            //just comparing distance sizes, so use squared mag, tiny performance thing
            float activeDist = (activeTransform.ObservedTransform.position - other.position).sqrMagnitude;
            float myDist = (transform.position - other.position).sqrMagnitude;

            //Check to see if this is the new closest. Note that this assignment will trigger an update to all subscribers
            if(myDist < activeDist)
                activeTransform.ObservedTransform = transform;
        }

        /// <summary>
        /// Update the state depending on of this is the new closest or not
        /// </summary>
        /// <param name="other"></param>
        public void NotifyNewClosest(Transform other)
        {
            if (other == transform)
            {
                onActive?.Invoke();
            }
            else
            {
                onInactive?.Invoke();
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Editor helper: draw lines with distances labeled in the scene to show which item/bot/whatever is closest to the tracked object (player)
        /// </summary>
        private void OnDrawGizmos()
        {
            if (Application.isPlaying && trackedTransform.ObservedTransform != null)
            {
                Gizmos.DrawLine(transform.position, trackedTransform.ObservedTransform.position);
                Vector3 v = transform.position - trackedTransform.ObservedTransform.position;
                Vector3 mid = trackedTransform.ObservedTransform.position + v * .5f;
                UnityEditor.Handles.Label(mid, $"{v.magnitude:0.##}");
            }
        }
#endif
    }
}
