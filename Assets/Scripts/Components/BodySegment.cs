using System;
using Unity.Collections;
using Unity.Entities;

[Serializable]
public struct BodySegment : IComponentData
{
    public Direction direction;
}
