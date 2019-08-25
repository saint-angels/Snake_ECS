using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Movable : IComponentData
{
    public Direction direction;
}
