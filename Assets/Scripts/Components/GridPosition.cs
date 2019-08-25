using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct GridPosition : IComponentData
{
    public int2 Value;
    public int layer;
}
