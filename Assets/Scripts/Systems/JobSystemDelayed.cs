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
    //TODO: REMOVE!!!
    protected EntityQuery spawnerQuery;
    protected SpawnerGardenEntity spawner;
    
    private float currentCooldown;

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
            spawner = spawnerArray[0];
            spawnerArray.Dispose();
            
            JobHandle compoundJobHandle = DelayedUpdate(inputDependencies);
            return compoundJobHandle;
        }
        else
        {
            currentCooldown -= UnityEngine.Time.deltaTime;
            return inputDependencies;
        }
    }

    protected abstract JobHandle DelayedUpdate(JobHandle inputDependencies);
}

public abstract class JobSystemDelayedWithBuffer : JobSystemDelayed
{
    protected EntityCommandBuffer.Concurrent beginInitCommandBuffer;
    protected EntityCommandBuffer.Concurrent endSimulationCommandBuffer;

    private EntityCommandBufferSystem endSimulationBarrier;
    private EntityCommandBufferSystem beginInitBarrier;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        endSimulationBarrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        beginInitBarrier = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override JobHandle DelayedUpdate(JobHandle inputDependencies)
    {
        endSimulationCommandBuffer = endSimulationBarrier.CreateCommandBuffer().ToConcurrent();
        beginInitCommandBuffer = beginInitBarrier.CreateCommandBuffer().ToConcurrent();
        
        JobHandle jobHandle = DelayedUpdateBuffer(inputDependencies);
        
        beginInitBarrier.AddJobHandleForProducer(jobHandle);
        endSimulationBarrier.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
    
    protected abstract JobHandle DelayedUpdateBuffer(JobHandle inputDependencies);
}
