using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class GridHelpers
{
    public static int2 DirectionToInt2(Direction direction)
    {
        switch (direction)
        {
            case Direction.UP:
                return new int2(0, 1);
            case Direction.DOWN:
                return new int2(0, -1);
            case Direction.LEFT:
                return new int2(-1, 0);
            case Direction.RIGHT:
                return new int2(1, 0);
            default:
                UnityEngine.Debug.LogError("unknown direction");
                return new int2(0, 0);
        }
    }

    public static Direction VectorToDirection(Vector2 vector)
    {
        return Direction.UP;
    }
}
