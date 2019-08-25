using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class SpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject prefabEarth;
    public GameObject prefabFollowJuice;
    
    // Referenced prefabs have to be declared so that the conversion system knows about them ahead of time
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(prefabEarth);
        referencedPrefabs.Add(prefabFollowJuice);
    }
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var spawnerData = new SpawnerGardenEntity
        {
            prefabEarth = conversionSystem.GetPrimaryEntity(prefabEarth),
            prefabFollowJuice = conversionSystem.GetPrimaryEntity(prefabFollowJuice),
        };

        dstManager.AddComponentData(entity, spawnerData);
    }
}