using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace verb
{
    /// <summary>
    /// Data object that contains a reference to a transform. This object also contains an action that may be subscribed to.
    /// Other objects may use this to notify all subscribers of any changes related to the transform or reference within.
    /// </summary>
    public class ObservableTransform : ScriptableObject
    {
        /// <summary>
        /// Action to notify all subscribers and send them the new transform.
        /// </summary>
        public Action<Transform> onObservedTransformChanged;

        /// <summary>
        /// Transform being "tracked" by this object
        /// </summary>
        protected Transform observedTransform = null;

        /// <summary>
        /// Property to get or update the transform reference contained within this object.
        /// Assigning a new transform will notify all subscribers of the change.
        /// </summary>
        public Transform ObservedTransform {
            get => observedTransform;
            set {
                observedTransform = value;
                Publish();
            }
        }

        /// <summary>
        /// Publish a notification to all subscribers that something about the transform has been changed
        /// </summary>
        public void Publish()
        {
            onObservedTransformChanged?.Invoke(observedTransform);
        }

#if UNITY_EDITOR
        /// <summary>
        /// ScriptableObject-ism; clear the stored reference to null between play mode runs while testing in the editor
        /// </summary>
        public void OnEnable()
        {
            observedTransform = null;
        }
#endif
    }
}
