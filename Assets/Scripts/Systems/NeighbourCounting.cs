using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;
using Random = Unity.Mathematics.Random;

public abstract class NeighbourCounting<T1, T2> : JobSystemDelayed
    where T1 : struct, ComponentWithValue
    where T2 : struct, IComponentData
{
    [BurstCompile]
    struct NeighbourCountingJob : IJobForEach<GridPosition, T1>
    {
        [ReadOnly] public NativeHashMap<int2, bool> gridEntities;
        
        public void Execute([ReadOnly] ref GridPosition gridPosition, ref T1 neighboursCounter)
        {
            int neighbourCount = 0;

            for (int x = gridPosition.Value.x - 1; x <= gridPosition.Value.x + 1; x++)
            {
                for (int y = gridPosition.Value.y - 1; y <= gridPosition.Value.y + 1; y++)
                {
                    bool neighbourPresent;
                    if (gridEntities.TryGetValue(new int2(x, y), out neighbourPresent))
                    {
                        neighbourCount++;
                    }
                }
            }

            neighboursCounter.Value = neighbourCount;
        }
    }
    
    private EntityQuery targetNeighbourQuery;

    protected override void OnCreate()
    {
        base.OnCreate();
        targetNeighbourQuery = GetEntityQuery(typeof(T2), typeof(GridPosition));
    }

    protected override JobHandle DelayedUpdate(JobHandle inputDependencies, SpawnerGardenEntity spawner)
    {
        NativeArray<GridPosition> gridEntityPositions = targetNeighbourQuery.ToComponentDataArray<GridPosition>(Allocator.TempJob);

        NativeHashMap<int2, bool> targetComponetPositions = new NativeHashMap<int2, bool>(targetNeighbourQuery.CalculateEntityCount(), Allocator.TempJob);

        for (int i = 0; i < gridEntityPositions.Length; i++)
        {
            targetComponetPositions.TryAdd(gridEntityPositions[i].Value, true);
        }

        var neighbourCountingJob = new NeighbourCountingJob
        {
            gridEntities = targetComponetPositions,
        };
        
        gridEntityPositions.Dispose();

        var jobHandle = neighbourCountingJob.Schedule(this, inputDependencies);

        return targetComponetPositions.Dispose(jobHandle);
    }
}