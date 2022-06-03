using UnityEngine;
using System;

public class QuantumPatch:Patch
{
    int numQubit = 1;
    QuantumState state;

    public QuantumPatch():base(new Vector2[] { Vector2.zero})
    {
        state = new QuantumState();
        SetEdgeGate(Vector2.zero, Direction.Right, new QEdgeProperty("X",false));
        SetEdgeGate(Vector2.zero, Direction.Left, new QEdgeProperty("X",false));
        SetEdgeGate(Vector2.zero, Direction.Up, new QEdgeProperty("Z",false));
        SetEdgeGate(Vector2.zero, Direction.Down, new QEdgeProperty("Z",false));

    }

    public QuantumPatch(Vector2[] coordinates,int numQubit,QuantumState state):base(coordinates)
    {
        this.numQubit = numQubit;
        this.state = state;
    }

    public bool SetEdgeGate(Vector2 coords, Direction direction, QEdgeProperty property)
    {
        var tmp = this.cellMap[Vector2Int.FloorToInt(coords)].edges[(int)direction].property;
        if(tmp == null || tmp is QEdgeProperty qeprop && !qeprop.oqqupied) {
            tmp = property;
            return true;
        }
        return false;
    }
}
