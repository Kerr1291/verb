using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace verb
{
    public class StandardHighlightableMonoBehaviour : AbstractHighlightableMonoBehaviour
    {
        new protected MeshRenderer renderer;
        public MeshRenderer Renderer {
            get {
                if (renderer == null)
                    renderer = GetComponentInChildren<MeshRenderer>(true);
                return renderer;
            }
        }

        [ContextMenu("Clear Highlight")]
        public override void ClearHighlight()
        {
            Renderer.SetStandardShaderColor(stateColors.defaultColor);
        }

        [ContextMenu("Enable Highlight")]
        public override void EnableHighlight()
        {
            Renderer.SetStandardShaderColor(stateColors.highlightColor);
        }
    }
}
