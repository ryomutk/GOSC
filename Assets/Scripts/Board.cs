using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class Board
{
    Cell[,] board;
    List<Patch> patches;
    Vector2Int extent;                                                                                                                                           

    public Board(Vector2 extent)
    {

        this.extent = Vector2Int.FloorToInt(extent);
        board = new Cell[this.extent.x, this.extent.y];
    }



    public Patch Get(Vector2 coordinate)
    {
        var cell = GetCell(coordinate);
        return patches.Find(x => x.cellMap.Values.Contains(cell));
    }

    public Cell GetCell(Vector2 coordinate)
    {
        var cords = Vector2Int.FloorToInt(coordinate);
        return board[cords.x, cords.y];
    }

    public bool Check(Vector2 coordinate)
    {
        return GetCell(coordinate) != null;
    }

    public bool Check(Vector2 coordinate,Patch patch)
    {
        foreach(var coord in patch.cellMap.Keys)
        {
            if(!Check(coord + coordinate))
            {
                return false;
            }
        }

        return true;
    }

    public bool Register(Vector2 coordinate,Patch patch)
    {
        var origin = Vector2Int.FloorToInt(coordinate);
        if (!Check(coordinate, patch))
        {
            foreach (var item in patch.cellMap)
            {
                var local = Vector2Int.FloorToInt(item.Key);
                var coord = local + origin;

                board[coord.x, coord.y] = item.Value;
            }
            return true;
        }

        return false;
    }
}
