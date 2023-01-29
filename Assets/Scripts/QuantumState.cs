using UnityEngine;
using System;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics;
using System.Numerics;

public class QuantumState
{
    //public Vector3 stateVector;
    public MathNet.Numerics.LinearAlgebra.Vector<Complex> stateVector;
    
    public QuantumState(Pauli eigenBase)
    {
        this.stateVector = QuantumMath.GetAugVector(eigenBase).Row(0);    
        Debug.Log(stateVector);
    }

    public QuantumState(MathNet.Numerics.LinearAlgebra.Vector<Complex> stateVector)
    {
        this.stateVector = stateVector;
    }
}
