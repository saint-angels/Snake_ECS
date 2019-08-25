using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]

public class GardenEntityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public EntityType entityType;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData<GridPosition>(entity, 
            new GridPosition
            {
                layer = Root.ConfigManager.LayersConfig.LayerForEntity(entityType)
            });
        
        switch (entityType)
        {
            case EntityType.EARTH:
                dstManager.AddComponentData(entity, new Earth());
                break;
            case EntityType.HEAD:
                dstManager.AddComponentData(entity, new HeadTag());
                dstManager.AddComponentData(entity, new Movable { direction = Direction.UP});
                break;
            case EntityType.FOLLOW_JUICE:
                dstManager.AddComponentData(entity, new Juice {lifetimeTicks = 4});
                break;
            case EntityType.BODY:
                dstManager.AddComponentData(entity, new BodySegment { });
                dstManager.AddComponentData(entity, new Movable { direction = Direction.UP});
                break;
        }
    }
}
