using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace verb
{
    /// <summary>
    /// Class that contaions helper methods for working with Vector2
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector3 ToVector3XZ(this Vector2 value, float y)
        {
            return new Vector3(value.x, y, value.y);
        }
    }
}