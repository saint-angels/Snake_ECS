using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class PlayerInput : MonoBehaviour
{
    public event Action<Vector2Int> OnDirectionPressed = (direction) => { };


    public Vector2Int? vectorPressed = null;
    public Direction directionPressed = Direction.NONE;

    public Direction GetPressedDirection()
    {
        if (directionPressed != Direction.NONE)
        {
            Direction buffer = directionPressed;
            directionPressed = Direction.NONE;
            return buffer;
        }
        else
        {
            return Direction.NONE;
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            vectorPressed = Vector2Int.down;
            directionPressed = Direction.DOWN;
            OnDirectionPressed(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            vectorPressed = Vector2Int.up;
            directionPressed = Direction.UP;
            OnDirectionPressed(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            directionPressed = Direction.RIGHT;
            vectorPressed = Vector2Int.right;

            OnDirectionPressed(Vector2Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            vectorPressed = Vector2Int.left;
            directionPressed = Direction.LEFT;
            OnDirectionPressed(Vector2Int.left);
        }
    }
}
