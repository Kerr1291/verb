using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace verb
{
    /// <summary>
    /// Data that contains a set of colors used in highlighting
    /// </summary>
    public class HighlightColorData : ScriptableObject
    {
        public Color defaultColor = Color.white;
        public Color highlightColor = Color.red;
    }
}
