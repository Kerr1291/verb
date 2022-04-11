using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace verb
{
    /// <summary>
    /// Class that contaions helper methods for working with MeshRenderer
    /// </summary>
    public static class MeshRendererExtensions
    {
        /// <summary>
        /// standard shader property ID for changing the color
        /// </summary>
        const string standardShaderColorID = "_Color";

        /// <summary>
        /// Method to change the color of a mesh renderer using the standard shader. NOTE: Does not check to see if the material is using the standard shader...
        /// </summary>
        /// <param name="renderer">Mesh renderer to act upon</param>
        /// <param name="newColor">Color to set the standard shader material to</param>
        public static void SetStandardShaderColor(this MeshRenderer renderer, Color newColor)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(mpb);
            mpb.SetColor(standardShaderColorID, newColor);
            renderer.SetPropertyBlock(mpb);
        }
    }
}
