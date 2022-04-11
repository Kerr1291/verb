using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace verb
{
    /// <summary>
    /// Base class for an MonoBehaviour that could be "highlighted"
    /// </summary>
    public abstract class AbstractHighlightableMonoBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Set of colors that would be used to turn on the highlight on/off
        /// </summary>
        public HighlightColorData stateColors;

        /// <summary>
        /// Disable the highlight effect
        /// </summary>
        public abstract void ClearHighlight();

        /// <summary>
        /// Enable the highlight effect
        /// </summary>
        public abstract void EnableHighlight();

        /// <summary>
        /// By default, set the color to the default/disabled color. SubClasses may override this if a different behaviour is desired.
        /// </summary>
        protected virtual void Awake()
        {
            ClearHighlight();
        }
    }
}
