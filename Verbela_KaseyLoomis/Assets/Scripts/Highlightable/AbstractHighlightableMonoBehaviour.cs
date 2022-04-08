using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace verb
{
    public abstract class AbstractHighlightableMonoBehaviour : MonoBehaviour
    {
        public HighlightColorData stateColors;

        public abstract void ClearHighlight();

        public abstract void EnableHighlight();

        protected virtual void Awake()
        {
            ClearHighlight();
        }
    }
}
