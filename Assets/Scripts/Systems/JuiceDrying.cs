using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class JuiceDrying : JobSystemDelayedWithBuffer
{
//    [BurstCompile]
    struct JuiceDryingJob : IJobForEachWithEntity<Juice>
    {
        [WriteOnly] public EntityCommandBuffer.Concurrent CommandBuffer;
        
        public void Execute(Entity entity, int index, ref Juice juice)
        {
            juice.lifetimeTicks--;

            if (juice.lifetimeTicks <= 0)
            {
                CommandBuffer.DestroyEntity(index, entity);
            }
        }
    }

    protected override JobHandle DelayedUpdateBuffer(JobHandle inputDependencies)
    {
        JobHandle jobHandle = new JuiceDryingJob
        {
            CommandBuffer = endSimulationCommandBuffer,
        }.Schedule(this, inputDependencies);
        
        return jobHandle;   
    }
}