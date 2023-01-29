using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using System.Numerics;

public class QuantumPatch : Patch
{
    int numQubit = 1;
    public QuantumState state { get; private set; }


    public QuantumPatch(UnityEngine.Vector2[] coords, Dictionary<UnityEngine.Vector2, Direction[]> smoothEdges, MathNet.Numerics.LinearAlgebra.Vector<Complex> initialState) : base(coords)
    {
        var vecs = new UnityEngine.Vector2[4] { UnityEngine.Vector2.up, UnityEngine.Vector2.right, UnityEngine.Vector2.down, UnityEngine.Vector2.left };
        state = new QuantumState(initialState);
        foreach (var coordDirection in smoothEdges)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (coords.Contains(vecs[(int)direction] + coordDirection.Key))
                {
                    SetEdgeGate(coordDirection.Key, direction, new QuantumEdgeProperty("", true));
                }
                else if (coordDirection.Value.Contains(direction))
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

    public QuantumPatch(UnityEngine.Vector2[] coords, Dictionary<UnityEngine.Vector2, Direction[]> smoothEdges, Pauli eigenBase) : base(coords)
    {
        state = new QuantumState(eigenBase);
        SetEdges(smoothEdges);
    }

    public void SetEdges(Dictionary<UnityEngine.Vector2, Direction[]> smoothEdges)
    {
        var vecs = new UnityEngine.Vector2[4] { UnityEngine.Vector2.up, UnityEngine.Vector2.right, UnityEngine.Vector2.down, UnityEngine.Vector2.left };
        foreach (var coordDirection in smoothEdges)
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (base.cellMap.Keys.Contains(vecs[(int)direction] + coordDirection.Key))
                {
                    SetEdgeGate(coordDirection.Key, direction, new QuantumEdgeProperty("", true));
                }
                else if (coordDirection.Value.Contains(direction))
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

    public QuantumPatch(UnityEngine.Vector2[] coordinates, int numQubit, QuantumState state) : base(coordinates)
    {
        this.numQubit = numQubit;
        this.state = state;
    }

    public bool SetEdgeGate(UnityEngine.Vector2 coords, Direction direction, QuantumEdgeProperty property)
    {
        EdgeProperty tmp = this.cellMap[UnityEngine.Vector2Int.FloorToInt(coords)].edges[(int)direction].property;
        if (tmp == null || tmp is QuantumEdgeProperty qeprop && !qeprop.oqqupied)
        {
            this.cellMap[UnityEngine.Vector2Int.FloorToInt(coords)].edges[(int)direction].property = property;
            return true;
        }
        return false;
    }

    public string GetInfo()
    {
        string msg = "State:" + "[" + state.stateVector[0].Real + "+" + state.stateVector[0].Imaginary + "i" + "," + state.stateVector[1].Real + "+" + state.stateVector[1].Imaginary + "i" + "]" + "<br>" + "Size:" + cellMap.Count() + "<br>" + "Edges:" + "<br>";
        foreach (var cell in cellMap)
        {
            msg += "  " + cell.Key + ":<br>";

            foreach (int direction in Enum.GetValues(typeof(Direction)))
            {
                var edge = cell.Value.edges[direction];
                msg += "    " + (Direction)direction + (edge.property as QuantumEdgeProperty).operatorType + "<br>";
            }
        }

        return msg;
    }

    public string getStateInfo(DenseMatrix baseMatrix)
    {
        var result = StateInBase(baseMatrix);
        var msg = "";
        var count = 0;
        foreach (var cv in result)
        {
            if (count == 0)
            {
                msg += (cv.Value.Real != 0 ? QuantumMath.replaceF(cv.Value.Real) : "" + (cv.Value.Imaginary > 0 ? " + " : " ")) + (cv.Value.Imaginary != 0 ? QuantumMath.replaceF(cv.Value.Imaginary) : "") + (cv.Value.Magnitude > 0 ? QuantumMath.GetStateKet(stateVector: cv.Key, "|a>") : "");
            }
            else
            {
                msg += (cv.Value.Real != 0 ? (cv.Value.Real > 0 ? " + " : " ") + QuantumMath.replaceF(cv.Value.Real) : " ") + (cv.Value.Imaginary != 0 ? (cv.Value.Imaginary < 0 ? " + " : " ") + QuantumMath.replaceF(cv.Value.Imaginary) : "") + (cv.Value.Magnitude != 0 ? QuantumMath.GetStateKet(stateVector: cv.Key, "|a>") : "");
            }

            count++;
        }
        return msg;
    }


    public Dictionary<MathNet.Numerics.LinearAlgebra.Vector<Complex>, Complex> StateInBase(DenseMatrix baseMatrix)
    {
        return QuantumMath.StateInBase(this.state.stateVector, baseMatrix);
    }
}
