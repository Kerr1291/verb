using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extensions
{
    public static Vector3 ToVector3XZ(this Vector2 value, float y)
    {
        return new Vector3(value.x, y, value.y);
    }
}