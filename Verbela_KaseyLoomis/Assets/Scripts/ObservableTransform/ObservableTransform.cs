using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace verb
{
    public class ObservableTransform : ScriptableObject
    {
        public Action<Transform> onObservedTransformChanged;

        protected Transform observedTransform = null;
        public Transform ObservedTransform {
            get => observedTransform;
            set {
                observedTransform = value;
                Publish();
            }
        }

        public void Publish()
        {
            onObservedTransformChanged?.Invoke(observedTransform);
        }
    }
}
