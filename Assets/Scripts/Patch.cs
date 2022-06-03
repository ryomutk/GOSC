using System.Collections;
using System.Collections.ObjectModel; 
using System.Collections.Generic;
using UnityEngine;
using System;

public class Patch
{
    int _numQubit = 1;
    Dictionary<Vector2, Cell> _cellMap;
    public ReadOnlyDictionary<Vector2,Cell> cellMap { get;  }
    public Vector2 extent { get; }

    public Patch(Vector2[] coords)
    {
        foreach(var coord in coords)
        {
            _cellMap[coord] = new Cell();
        }
        cellMap = new ReadOnlyDictionary<Vector2, Cell>(_cellMap);
    }

}
