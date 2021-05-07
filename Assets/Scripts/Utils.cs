using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HackathonUtils
{
    public static class Utils
    {
        public static Vector2 GetPointOnCircle(Vector2 origin, float radius, float angle)
        {

            float angleInRadians = angle * Mathf.Deg2Rad;

            var x = origin.x + radius * Mathf.Sin(angleInRadians);
            var y = origin.y + radius * Mathf.Cos(angleInRadians);

            return new Vector2(x, y);

        }
    }
}

