using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class GridMover : JobSystemDelayedWithBuffer
{
    struct GridMovingJob : IJobForEachWithEntity<Translation, GridPosition, Movable>
    {
        [WriteOnly] public EntityCommandBuffer.Concurrent CommandBuffer;
        [ReadOnly] public SpawnerGardenEntity spawner;

        public void Execute(Entity entity, int index, ref Translation translation, ref GridPosition gridPosition, [ReadOnly] ref Movable movable)
        {
            if (movable.direction == Direction.NONE)
            {
                //Entity is static
                return;
            }
            
            //Spawn follow juice
            Entity newJuiceEntity = CommandBuffer.Instantiate(index, spawner.prefabFollowJuice);
            int layer = Root.ConfigManager.LayersConfig.LayerForEntity(EntityType.FOLLOW_JUICE);
            int2 juiceGridPosition = gridPosition.Value;
            float3 juiceTranslation = GridConfig.PositionForCoordinates(juiceGridPosition.x, juiceGridPosition.y, layer);
            CommandBuffer.SetComponent(index, newJuiceEntity, new GridPosition { Value = juiceGridPosition, layer = layer});
            CommandBuffer.SetComponent(index, newJuiceEntity, new Translation() { Value = juiceTranslation});
            
            
            int2 offset = GridHelpers.DirectionToInt2(movable.direction);
            //Wrap around the grid
            int wrappedX = gridPosition.Value.x + offset.x;
            int wrappedY = gridPosition.Value.y + offset.y;
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
    

    protected override JobHandle DelayedUpdateBuffer(JobHandle inputDependencies)
    {
        var job = new GridMovingJob
        {
            CommandBuffer = beginInitCommandBuffer,
            spawner = spawner
        };
        return job.Schedule(this, inputDependencies);
    }
}