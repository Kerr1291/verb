using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace verb
{
    public class TransformDistanceBehaviour : MonoBehaviour
    {
        public ObservableTransform trackedTransform;
        public ObservableTransform activeTransform;

        public UnityEngine.Events.UnityEvent onActive;
        public UnityEngine.Events.UnityEvent onInactive;

        public void OnEnable()
        {
            trackedTransform.onObservedTransformChanged -= CheckClosest;
            trackedTransform.onObservedTransformChanged += CheckClosest;

            activeTransform.onObservedTransformChanged -= NotifyNewClosest;
            activeTransform.onObservedTransformChanged += NotifyNewClosest;
        }

        public void OnDisable()
        {
            trackedTransform.onObservedTransformChanged -= CheckClosest;
            activeTransform.onObservedTransformChanged -= NotifyNewClosest;
        }

        public void CheckClosest(Transform other)
        {
            if(activeTransform.ObservedTransform == null)
            {
                activeTransform.ObservedTransform = transform;
                return;
            }

            if (activeTransform.ObservedTransform == transform)
                return;

            float activeDist = (activeTransform.ObservedTransform.position - trackedTransform.ObservedTransform.position).sqrMagnitude;
            float myDist = (transform.position - trackedTransform.ObservedTransform.position).sqrMagnitude;

            if(myDist < activeDist)
                activeTransform.ObservedTransform = transform;
        }

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
