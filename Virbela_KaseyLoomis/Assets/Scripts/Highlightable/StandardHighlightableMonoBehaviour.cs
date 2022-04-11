using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace verb
{
    /// <summary>
    /// Contains an implementation of AbstractHighlightableMonoBehaviour that defines how to highlight a MeshRenderer using the StandardShader
    /// </summary>
    public class StandardHighlightableMonoBehaviour : AbstractHighlightableMonoBehaviour
    {
        /// <summary>
        /// The unity default renderer keyword is obsolete so let's briefly use this name that pairs nicely with our property name.
        /// </summary>
        new protected MeshRenderer renderer;

        /// <summary>
        /// Defines a lazyily instantiated reference to a MeshRenderer contained on this object OR A CHILD of this object.
        /// </summary>
        public MeshRenderer Renderer {
            get {
                if (renderer == null)
                    renderer = GetComponentInChildren<MeshRenderer>(true);
                return renderer;
            }
        }

        /// <summary>
        /// Set the highlight color of the standard shader to the default color
        /// </summary>
        [ContextMenu("Clear Highlight")]
        public override void ClearHighlight()
        {
            Renderer.SetStandardShaderColor(stateColors.defaultColor);
        }

        /// <summary>
        /// Set the highlight color of the standard shader to the highlight color
        /// </summary>
        [ContextMenu("Enable Highlight")]
        public override void EnableHighlight()
        {
            Renderer.SetStandardShaderColor(stateColors.highlightColor);
        }
    }
}
