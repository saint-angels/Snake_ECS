﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class HeadMoveSystem : JobSystemDelayedWithBuffer
{
//    [BurstCompile]
    struct HeadMoveSystem2Job : IJobForEachWithEntity<GridPosition, Head, Translation>
    {
        [WriteOnly] public EntityCommandBuffer.Concurrent CommandBuffer;
        [ReadOnly] public SpawnerGardenEntity spawner;
        [ReadOnly] public Direction buttonDireciton;
        
        public void Execute(Entity entity, int index, ref GridPosition gridPosition, ref Head head, ref Translation translation)
        {
            //Spawn follow juice
            Entity newJuiceEntity = CommandBuffer.Instantiate(index, spawner.prefabFollowJuice);
//            CommandBuffer.AddComponent<Juice>(index, newJuiceEntity, new Juice {lifetimeTicks = 4});
            int layer = Root.ConfigManager.LayersConfig.LayerForEntity(EntityType.FOLLOW_JUICE);
            int2 juicePosition = gridPosition.Value;
            CommandBuffer.SetComponent(index, newJuiceEntity, new GridPosition { Value = juicePosition, layer = layer});
            CommandBuffer.SetComponent(index, newJuiceEntity, new Translation() { Value = GridConfig.PositionForCoordinates(juicePosition.x, juicePosition.y, gridPosition.layer) });
            
            if (buttonDireciton != Direction.NONE)
            {
                head.direction = SwitchDirection(head.direction, buttonDireciton);
            }
            int2 offset = GridHelpers.DirectionToInt2(head.direction);
            gridPosition.Value = gridPosition.Value + new int2(offset.x, offset.y);
//            translation.Value = GridConfig.PositionForCoordinates(gridPosition.Value.x, gridPosition.Value.y, gridPosition.worldLayer);
        }
    }

    protected override JobHandle DelayedUpdateBuffer(JobHandle inputDependencies, SpawnerGardenEntity spawner, EntityCommandBuffer.Concurrent buffer)
    {
        Direction buttonDirection = Root.PlayerInput.GetPressedDirection();

        JobHandle newJobHandle = new HeadMoveSystem2Job
        {
            spawner = spawner,
            CommandBuffer = buffer,
            buttonDireciton = buttonDirection
        }.Schedule(this, inputDependencies);

        return newJobHandle;
    }
    
    private static Direction SwitchDirection(Direction original, Direction buttonDirection)
    {
        Direction result = original;
        switch (original)
        {
            case Direction.UP:
            case Direction.DOWN:
                switch (buttonDirection)
                {
                    case Direction.LEFT:
                        result = Direction.LEFT;
                        break;
                    case Direction.RIGHT:
                        result = Direction.RIGHT;
                        break;
                }
                break;
            case Direction.LEFT:
            case Direction.RIGHT:
                switch (buttonDirection)
                {
                    case Direction.UP:
                        result = Direction.UP;
                        break;
                    case Direction.DOWN:
                        result = Direction.DOWN;
                        break;
                }
                break;
        }

        return result;
    }

}