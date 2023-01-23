using UnityEngine;
using System;
using System.Numerics;

public class QuantumState
{
    //public Vector3 stateVector;
    public Complex[] stateVector = new Complex[2];
    

    public QuantumState()
    {
        
    }

    public QuantumState(Complex[] stateVector)
    {
        this.stateVector = stateVector;
    }
}
