using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
[Serializable]
public class Board
{
    [ShowInInspector, TableMatrix(DrawElementMethod = "Read Only Matrix"), ReadOnly] Cell[,] board;
    Dictionary<Patch, Vector2Int> _patchMap = new Dictionary<Patch, Vector2Int>();
    Vector2Int _extent;
    public Vector2Int extent { get { return _extent; } }
    public Dictionary<Patch, Vector2Int> patchMap { get { return _patchMap; } }
    public bool[,] selected { get; private set; }

    public Dictionary<Patch, List<Edge>> GetTangentEdges(Patch a, Patch b)
    {
        Dictionary<Patch, List<Edge>> results = new Dictionary<Patch, List<Edge>>();
        var aCoords = patchMap[a];
        var bCoords = patchMap[b];
        results[a] = new List<Edge>();
        results[b] = new List<Edge>();
        foreach (var cell in a.cellMap)
        {
            var local = aCoords + cell.Key;
            Vector2 directionVec;
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                directionVec = DirectionConv.dirToVec(direction);

                var target = directionVec + local;

                var aEdge = cell.Value.edges[(int)direction];

                var neibor = GetCell(target);

                if (neibor != null && !aEdge.property.oqqupied)
                {
                    results[a].Add(item: aEdge);
                    var opposite = directionVec * -1;
                    var dir = (int)DirectionConv.vecToDir(opposite);
                    results[b].Add(neibor.edges[dir]);
                }
            }
        }

        return results;
    }

    public Board(Vector2 extent)
    {
        this._extent = Vector2Int.FloorToInt(extent);
        board = new Cell[this._extent.x, this._extent.y];
        selected = new bool[this._extent.x, this._extent.y];
    }

    public void PatchSelect(Patch target, bool off = false)
    {
        var origin = patchMap[target];
        foreach (Vector2 pos in target.cellMap.Keys)
        {
            selected[origin.x + (int)pos.x, origin.y + (int)pos.y] = !off;
        }
    }

    public void DeselectAll()
    {
        selected = new bool[this._extent.x, this._extent.y];
    }



    public Patch Get(Vector2 coordinate)
    {
        var cell = GetCell(coordinate);
        return _patchMap.First(x => x.Key.cellMap.Values.Any(x => x == cell)).Key;
    }

    public Cell GetCell(Vector2 coordinate)
    {
        var cords = Vector2Int.FloorToInt(coordinate);
        try
        {
            return board[cords.x, cords.y];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }

    public bool Check(Vector2 coordinate)
    {
        return GetCell(coordinate) != null;
    }

    public bool Check(Vector2 coordinate, Patch patch)
    {
        foreach (var coord in patch.cellMap.Keys)
        {
            if (!Check(coord + coordinate))
            {
                return false;
            }
        }

        return true;
    }

    public bool RemovePatch(Patch patch)
    {
        var origin = patchMap[patch];
        if(_patchMap.Remove(patch))
        {
            foreach(var local in patch.cellMap.Keys)
            {
                var coord = origin+Vector2Int.FloorToInt(local);
                board[coord.x,coord.y] = null;
            }
        }
        return _patchMap.Remove(patch);
    }

    public bool Register(Vector2 coordinate, Patch patch)
    {
        var origin = Vector2Int.FloorToInt(coordinate);
        if (!Check(coordinate, patch))
        {
            _patchMap[patch] = Vector2Int.RoundToInt(coordinate);

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
