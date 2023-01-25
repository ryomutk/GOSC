using UnityEngine;
using System;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics;

public class QuantumState
{
    //public Vector3 stateVector;
    public MathNet.Numerics.LinearAlgebra.Vector<Complex32> stateVector;
    
    public QuantumState(Pauli eigenBase)
    {
        this.stateVector = QuantumMath.GetAugVector(eigenBase).Row(1);    
    }

    public QuantumState(MathNet.Numerics.LinearAlgebra.Vector<Complex32> stateVector)
    {
        this.stateVector = stateVector;
    }
}
