using System;
using UnityEngine;

public enum Direction : int
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}

public static class DirectionConv
{
    public static Vector2 dirToVec(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector2.up;
            case Direction.Right:
                return Vector2.right;
            case Direction.Down:
                return Vector2.down;
            case Direction.Left:
                return Vector2.left;
            default:
                throw new Exception();
        }
    }

    public static Direction vecToDir(Vector2 direction)
    {
        if(direction.magnitude==0)
        {
            throw new Exception();
        }
        if(direction.x == 1)
        {
            return Direction.Right;
        }
        else if(direction.x == -1)
        {
            return Direction.Left;
        }
        else if(direction.y == 1)
        {
            return Direction.Up;
        }
        else if(direction.y == -1)
        {
            return Direction.Down;
        }

        throw new Exception();
    }
}