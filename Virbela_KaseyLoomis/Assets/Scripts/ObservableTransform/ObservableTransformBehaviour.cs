using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace verb
{
    /// <summary>
    /// Unity object to allow for scene interaction with the ObservableTransform data object
    /// </summary>
    public class ObservableTransformBehaviour : MonoBehaviour
    {
        [Header("Allows both prefab assets and in-scene objects to subscribe to changes in the transform on this object")]
        /// <summary>
        /// Data object that tracks a transform reference and provides a way for others to subscribe to it
        /// </summary>
        public ObservableTransform onTransformUpdated;

        /// <summary>
        /// When this object is awaken, bind the transform on this game object to the data object
        /// </summary>
        protected virtual void Awake()
        {
            if (onTransformUpdated != null)
                onTransformUpdated.ObservedTransform = transform;
            transform.hasChanged = false;
        }

        /// <summary>
        /// Check each frame to see if the transform has changed and notify all subscribers
        /// </summary>
        protected virtual void Update()
        {
            if (onTransformUpdated != null && transform.hasChanged)
            {
                onTransformUpdated.Publish();
                transform.hasChanged = false;
            }
        }
    }
}
