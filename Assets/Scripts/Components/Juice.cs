using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Juice : IComponentData
{
    public int lifetimeTicks;
}
