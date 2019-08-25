using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class JuiceFollowSystem : JobSystemDelayedWithBuffer
{
    [ExcludeComponent(typeof(HeadTag))]
    struct JuiceFollowSystemJob : IJobForEachWithEntity<BodySegment, GridPosition, Movable>
    {
        [ReadOnly] public NativeHashMap<int2, Entity> juiceAtPositions;
        [WriteOnly] public EntityCommandBuffer.Concurrent commandBuffer;

        [ReadOnly] private static readonly int2[] neighbourOffsets = 
        {
            new int2(-1,0 ),
            new int2(1,0 ),
            new int2(0, -1 ),
            new int2(0, 1 ),
        };
        
        public void Execute(Entity entity, int index, ref BodySegment bodySegment, ref GridPosition gridPosition, ref Movable movable)
        {
            if (juiceAtPositions.TryGetValue(gridPosition.Value, out var currentPositionJuiceEntity))
            {
                commandBuffer.DestroyEntity(index, currentPositionJuiceEntity);
            }
            
            for (int i = 0; i < neighbourOffsets.Length; i++)
            {
                int2 neioghbourOffset = neighbourOffsets[i];
                if (juiceAtPositions.TryGetValue(gridPosition.Value + neioghbourOffset, out var neighbourJuiceEntity))
                {
                    movable.direction = OffsetToDirection(neioghbourOffset);
                    commandBuffer.DestroyEntity(index, neighbourJuiceEntity);
                    break;
                }
            }
        }

        private static Direction OffsetToDirection(int2 offset)
        {
            if (offset.x == -1)
            {
                return Direction.LEFT;
            } 
            else if (offset.x == 1)
            {
                return Direction.RIGHT;
            }
            else if (offset.y == -1)
            {
                return Direction.DOWN;
            }
            else
            {
                return Direction.UP;
            }
        }
    }
    
    private EntityQuery juiceEntitiesQuery;

    protected override void OnCreate()
    {
        base.OnCreate();
        juiceEntitiesQuery = GetEntityQuery(typeof(Juice), typeof(GridPosition));
    }

    protected override JobHandle DelayedUpdateBuffer(JobHandle inputDependencies)
    {
        NativeArray<GridPosition> gridEntityPositions = juiceEntitiesQuery.ToComponentDataArray<GridPosition>(Allocator.TempJob);
        var juiceEntities = juiceEntitiesQuery.ToEntityArray(Allocator.TempJob);
        NativeHashMap<int2, Entity> juiceAtPositions = new NativeHashMap<int2, Entity>(juiceEntitiesQuery.CalculateEntityCount(), Allocator.TempJob);
        
        for (int i = 0; i < gridEntityPositions.Length; i++)
        {
            juiceAtPositions.TryAdd(gridEntityPositions[i].Value, juiceEntities[i]);
        }

        var jobHandle = new JuiceFollowSystemJob
        {
           commandBuffer = beginInitCommandBuffer,
           juiceAtPositions = juiceAtPositions
        }.Schedule(this, inputDependencies);
        
        gridEntityPositions.Dispose();
        juiceEntities.Dispose();
        
        return juiceAtPositions.Dispose(jobHandle);
    }
}