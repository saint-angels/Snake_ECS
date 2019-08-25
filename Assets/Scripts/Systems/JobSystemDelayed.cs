using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Rendering;
using static Unity.Mathematics.math;

public abstract class JobSystemDelayed : JobComponentSystem
{
    float currentCooldown;
    protected EntityQuery spawnerQuery;

    protected override void OnCreate()
    {
        base.OnCreate();
        
        spawnerQuery = GetEntityQuery(typeof(SpawnerGardenEntity));
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        if (currentCooldown <= 0)
        {
            currentCooldown = Root.SimulationTick;
            
            NativeArray<SpawnerGardenEntity> spawnerArray = spawnerQuery.ToComponentDataArray<SpawnerGardenEntity>(Allocator.TempJob);
            SpawnerGardenEntity spawner = spawnerArray[0];
            spawnerArray.Dispose();
            
            JobHandle compoundJobHandle = DelayedUpdate(inputDependencies, spawner);
            return compoundJobHandle;
        }
        else
        {
            currentCooldown -= UnityEngine.Time.deltaTime;
            return inputDependencies;
        }
    }

    protected abstract JobHandle DelayedUpdate(JobHandle inputDependencies, SpawnerGardenEntity spawner);
}

public abstract class JobSystemDelayedWithBuffer : JobSystemDelayed
{
    private EntityCommandBufferSystem m_Barrier;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        m_Barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle DelayedUpdate(JobHandle inputDependencies, SpawnerGardenEntity spawner)
    {
        EntityCommandBuffer.Concurrent commandBuffer = m_Barrier.CreateCommandBuffer().ToConcurrent();
        JobHandle jobHandle = DelayedUpdateBuffer(inputDependencies, spawner, commandBuffer);
        m_Barrier.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
    
    protected abstract JobHandle DelayedUpdateBuffer(JobHandle inputDependencies, SpawnerGardenEntity spawner, EntityCommandBuffer.Concurrent commandBuffer);
}
