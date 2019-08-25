using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class GridWrapping : JobComponentSystem
{
    [BurstCompile]
    [RequireComponentTag(typeof(Moving))]
    struct GridWrappingJob : IJobForEach<Translation, GridPosition>
    {

        public void Execute(ref Translation translation, ref GridPosition gridPosition)
        {
            int wrappedX = gridPosition.Value.x;
            int wrappedY = gridPosition.Value.y;
            if (gridPosition.Value.x < 0)
            {
                wrappedX = GridConfig.width - 1;
            }
            else if (GridConfig.width <= gridPosition.Value.x)
            {
                wrappedX = 0;
            } 
            else if (gridPosition.Value.y < 0)
            {
                wrappedY = GridConfig.height - 1;
            }
            else if (GridConfig.height <= gridPosition.Value.y)
            {
                wrappedY = 0;
            } 
            
            gridPosition.Value = new int2(wrappedX, wrappedY);
            translation.Value = GridConfig.PositionForCoordinates(wrappedX, wrappedY, gridPosition.layer);
        }
    }
    
    private float3 startPoint;

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new GridWrappingJob
        {
        };
        return job.Schedule(this, inputDependencies);
    }
}