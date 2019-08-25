using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEditor.U2D;
using UnityEngine;
using Random = System.Random;

public class GardenCreator : MonoBehaviour
{
    struct RandomSelection
    {
        public Entity entityPrefab;
        public float probability;

        public RandomSelection(Entity entityPrefab, float probability)
        {
            this.entityPrefab = entityPrefab;
            this.probability = probability;
        }
    }


    [SerializeField] private GameObject headPrefab;
    [SerializeField] private GameObject earthPrefab;
    [SerializeField] private GameObject bodySegmentPrefab;

    private EntityManager entityManager;

    private void Start()
    {
        entityManager = World.Active.EntityManager;
        
        Entity earthEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(earthPrefab, World.Active);

//        var entityPrefabs = new RandomSelection[]
//        {
//            new RandomSelection(earthEntityPrefab, 1f), 
////            new RandomSelection(leafEntityPrefab, .7f), 
////            new RandomSelection(wormEntityPrefab, .1f) 
//        };
        
        //Generate grid
        
//        for (int x = 0; x < GridConfig.width; x++)
//        {
//            for (int y = 0; y < GridConfig.height; y++)
//            {
//                Entity randomPrefab = GetRandomValue(entityPrefabs);
//                Entity instance = entityManager.Instantiate(randomPrefab);
//                int z = Root.ConfigManager.LayersConfig.LayerForEntity(EntityType.EARTH);
//                entityManager.SetComponentData(instance, new GridPosition { Value = new int2(x, y),  worldLayer = z});
//                float3 position = GridConfig.PositionForCoordinates(x, y, z);
//                entityManager.SetComponentData(instance, new Translation {Value = position});
//            }
//        }
        
        //Put head on a grid
        Entity headEntityPrafab = GameObjectConversionUtility.ConvertGameObjectHierarchy(headPrefab, World.Active);
        Entity headEntityInstance = entityManager.Instantiate(headEntityPrafab);

        int2 gridPositionHead = new int2(Mathf.RoundToInt(GridConfig.width / 2f), Mathf.RoundToInt(GridConfig.height / 2f));
        int headVisualZ = Root.ConfigManager.LayersConfig.LayerForEntity(EntityType.HEAD);
        entityManager.SetComponentData(headEntityInstance, new GridPosition { Value = gridPositionHead, layer = headVisualZ});
        //Translation is set in GridWrapping system for Move-Entities
//        float3 positionHead = GridConfig.PositionForCoordinates(gridPositionHead.x, gridPositionHead.y, headVisualZ);
//        entityManager.SetComponentData(headEntityInstance, new Translation {Value = positionHead});
        
        //Add body segments
//        for (int i = 1; i <= 3; i++)
//        {
//            Entity bodySegmentEntityInstance = entityManager.Instantiate(GameObjectConversionUtility.ConvertGameObjectHierarchy(bodySegmentPrefab, World.Active));
//            entityManager.SetComponentData(bodySegmentEntityInstance, new GridPosition { Value = new int2(gridPositionHead.x, gridPositionHead.y - i), worldLayer = headVisualZ});
//        }
        
        
        Destroy(gameObject);
    }
    
    Entity GetRandomValue(params RandomSelection[] selections) {
        float rand = UnityEngine.Random.value;
        float currentProb = 0;
        foreach (var selection in selections) {
            currentProb += selection.probability;
            if (rand <= currentProb)
            {
                return selection.entityPrefab;
            }
        }
 
        //will happen if the input's probabilities sums to less than 1
        //throw error here if that's appropriate
        Debug.LogError("Probabilities less than 1");
        return Entity.Null;
    }
}