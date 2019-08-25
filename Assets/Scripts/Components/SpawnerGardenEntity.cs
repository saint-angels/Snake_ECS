using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct SpawnerGardenEntity : IComponentData
{
    public Entity prefabEarth;
    public Entity prefabFollowJuice;
    public Entity prefabSnakePart;

    public Entity GetEntityForType(EntityType type)
    {
        switch (type)
        {
            case EntityType.EARTH:
                return prefabEarth;
            case EntityType.FOLLOW_JUICE:
                return prefabFollowJuice;
            case EntityType.BODY:
                return prefabSnakePart;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
