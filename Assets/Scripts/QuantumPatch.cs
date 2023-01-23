using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class QuantumPatch : Patch
{
    int numQubit = 1;
    QuantumState state;

    public QuantumPatch(Vector2[] coords, Dictionary<Vector2, Direction[]> smoothEdges) : base(coords)
    {
        var vecs = new Vector2[4]{Vector2.up,Vector2.right,Vector2.down,Vector2.left};
        state = new QuantumState();
        foreach (var coordDirection in smoothEdges)
        {
            foreach(Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if(coords.Contains(vecs[(int)direction]+coordDirection.Key))
                {
                    SetEdgeGate(coordDirection.Key, direction, new QuantumEdgeProperty("", true));
                }
                else if(coordDirection.Value.Contains(direction))
                {
                    SetEdgeGate(coordDirection.Key, direction, new QuantumEdgeProperty("X", false));
                }
                else
                {
                    SetEdgeGate(coordDirection.Key, direction, new QuantumEdgeProperty("Z", false));
                }
            }
        }
    }

    public QuantumPatch(Vector2[] coordinates, int numQubit, QuantumState state) : base(coordinates)
    {
        this.numQubit = numQubit;
        this.state = state;
    }

    public bool SetEdgeGate(Vector2 coords, Direction direction, QuantumEdgeProperty property)
    {
        EdgeProperty tmp = this.cellMap[Vector2Int.FloorToInt(coords)].edges[(int)direction].property;
        if (tmp == null || tmp is QuantumEdgeProperty qeprop && !qeprop.oqqupied)
        {
            this.cellMap[Vector2Int.FloorToInt(coords)].edges[(int)direction].property = property;
            return true;
        }
        return false;
    }

    public string GetInfo()
    {
        var msg = "State:" +"["+state.stateVector[0].Real+"+" + state.stateVector[0].Imaginary +"i"+","+state.stateVector[1].Real +"+"+ state.stateVector[1].Imaginary +"i"+"]"+"<br>"+"Size:"+cellMap.Count()+"<br>"+"Edges:"+"<br>";
        foreach(var cell in cellMap)
        {
            msg += "  "+cell.Key +":<br>";
            
            foreach(int direction in Enum.GetValues(typeof(Direction)))
            {
                var edge = cell.Value.edges[direction];
                msg += "    " + (Direction)direction+(edge.property as QuantumEdgeProperty).operatorName+"<br>";
            }
        }

        return msg;
    }
}
