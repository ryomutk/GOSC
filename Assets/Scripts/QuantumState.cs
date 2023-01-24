using UnityEngine;
using System;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra.Complex;

public class QuantumState
{
    //public Vector3 stateVector;
    public MathNet.Numerics.LinearAlgebra.Vector<Complex> stateVector;
    
    public QuantumState(Pauli eigenBase)
    {
        this.stateVector = QuantumMath.GetAugVector(eigenBase).Row(1);    
    }

    public QuantumState(DenseMatrix eigenBase)
    {
        this.stateVector = QuantumMath.GetAugVector(eigenBase).Row(1);
    }
}
