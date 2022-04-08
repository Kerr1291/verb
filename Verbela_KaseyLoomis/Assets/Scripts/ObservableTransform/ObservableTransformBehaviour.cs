using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace verb
{
    public class ObservableTransformBehaviour : MonoBehaviour
    {
        public ObservableTransform onTransformUpdated;

        //public bool SendInitNotify = true;

        private void Awake()
        {
            if(onTransformUpdated != null)
                onTransformUpdated.ObservedTransform = transform;
            transform.hasChanged = false;
        }

        //IEnumerator Start()
        //{
        //    yield return new WaitForEndOfFrame();
        //    if (SendInitNotify)
        //    {
        //        if (onTransformUpdated != null)
        //            onTransformUpdated.Publish();
        //    }
        //}

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
