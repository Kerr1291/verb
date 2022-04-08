using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace verb
{
    public static class MeshRendererExtensions
    {
        const string standardShaderColorID = "_Color";

        public static void SetStandardShaderColor(this MeshRenderer renderer, Color newColor)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(mpb);
            mpb.SetColor(standardShaderColorID, newColor);
            renderer.SetPropertyBlock(mpb);
        }
    }
}
