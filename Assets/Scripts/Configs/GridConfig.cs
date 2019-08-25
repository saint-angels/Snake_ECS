using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class GridConfig
{

    public const int width = 20;
    public const int height = 20;

    public const float cellScale = 1f;

    private static readonly float3 startPoint = new float3(-(width / 2f) * cellScale, -(height / 2f) * cellScale, 0);

    public static float3 PositionForCoordinates(int x, int y, int z)
    {
        return new float3(x * cellScale + startPoint.x, y * cellScale + startPoint.y, z);   
    }
        
    
}
