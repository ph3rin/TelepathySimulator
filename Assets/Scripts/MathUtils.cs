using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal 
    
    class MathUtils
{
    public static float EvaluateGrowthCurve(float growth, float horizontalScale, float verticalScale)
    {
        return verticalScale * Mathf.Log(horizontalScale * growth + 1);
    }
}