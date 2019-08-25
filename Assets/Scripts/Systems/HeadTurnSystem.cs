using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;

//[UpdateAfter(typeof(GridWrapping))]
public class HeadTurnSystem : JobSystemDelayedWithBuffer
{
//    [BurstCompile]
    [RequireComponentTag(typeof(HeadTag))]
    struct HeadMoveSystem2Job : IJobForEachWithEntity<GridPosition, Movable, Translation>
    {
        [ReadOnly] public Direction buttonDireciton;
        public void Execute(Entity entity, int index, ref GridPosition gridPosition, ref Movable movable, ref Translation translation)
        {
            if (buttonDireciton != Direction.NONE)
            {
                movable.direction = SwitchDirection2Buttons(movable.direction, buttonDireciton);
            }
        }
    }

    protected override JobHandle DelayedUpdateBuffer(JobHandle inputDependencies)
    {
        Direction buttonDirection = Root.PlayerInput.GetPressedDirection();

        JobHandle newJobHandle = new HeadMoveSystem2Job
        {
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
    
    private static Direction SwitchDirection2Buttons(Direction original, Direction buttonDirection)
    {
        Direction result = original;
        switch (original)
        {
            case Direction.UP:
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
            case Direction.DOWN:
                switch (buttonDirection)
                {
                    case Direction.LEFT:
                        result = Direction.RIGHT;
                        break;
                    case Direction.RIGHT:
                        result = Direction.LEFT;
                        break;
                }
                break;
            case Direction.LEFT:
                switch (buttonDirection)
                {
                    case Direction.LEFT:
                        result = Direction.DOWN;
                        break;
                    case Direction.RIGHT:
                        result = Direction.UP;
                        break;
                }
                break;
            case Direction.RIGHT:
                switch (buttonDirection)
                {
                    case Direction.LEFT:
                        result = Direction.UP;
                        break;
                    case Direction.RIGHT:
                        result = Direction.DOWN;
                        break;
                }
                break;
        }

        return result;
    }

}