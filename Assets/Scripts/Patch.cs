using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Patch
{
    int _numQubit = 1;
    
    Dictionary<Vector2, Cell> _cellMap = new Dictionary<Vector2, Cell>();
    public ReadOnlyDictionary<Vector2, Cell> cellMap { get; }
    public ReadOnlyDictionary<Cell, AbstractShape> cellShapes;
    public Vector2 extent { get; }

    public Patch(Vector2[] coords)
    {
        foreach (var coord in coords)
        {
            _cellMap[coord] = new Cell();
        }
        cellMap = new ReadOnlyDictionary<Vector2, Cell>(_cellMap);
        cellShapes = new ReadOnlyDictionary<Cell, AbstractShape>(ShapeCells());
    }

    Dictionary<Cell, AbstractShape> ShapeCells()
    {
        var shapeMap = new Dictionary<Cell, AbstractShape>();
        foreach (var cordCell in cellMap)
        {
            var shape = new AbstractShape();
            shape.direction = Vector2.zero;

            var cord = cordCell.Key;
            if (cellMap.ContainsKey(cord + Vector2.up))
            {
                shape.direction += Vector2.up;
                shape.connectCount++;
            }
            if (cellMap.ContainsKey(cord + Vector2.left))
            {
                shape.direction += Vector2.left;
                shape.connectCount++;
            }
            if (cellMap.ContainsKey(cord + Vector2.down))
            {
                shape.direction += Vector2.down;
                shape.connectCount++;
            }
            if (cellMap.ContainsKey(cord + Vector2.right))
            {
                shape.direction += Vector2.right;
                shape.connectCount++;
            }

            if (shape.connectCount == 2 && shape.direction == Vector2.zero)
            {
                if (cellMap.ContainsKey(cord + Vector2.up))
                {
                    shape.isTopDown = true;
                }
            }
            shapeMap[cordCell.Value] = shape;
        }
#if DEBUG
        Debug.Log(String.Join(" ",shapeMap.Values));
#endif
        return shapeMap;
    }

}
