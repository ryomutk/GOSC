using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class Board
{
    Cell[,] board;
    Dictionary<Patch,Vector2Int> _patchMap;
    Vector2Int _extent;                                                                                                                                           
    public Vector2Int extent{get{return _extent;}}
    public Dictionary<Patch,Vector2Int> patchMap{get{return _patchMap;}}

    public Board(Vector2 extent)
    {

        this._extent = Vector2Int.FloorToInt(extent);
        board = new Cell[this._extent.x, this._extent.y];
    }



    public Patch Get(Vector2 coordinate)
    {
        var cell = GetCell(coordinate);
        return _patchMap.First(x => x.Key.cellMap.Values.Any(x => x ==cell)).Key;
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
