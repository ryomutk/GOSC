using UnityEngine;
using System;


public class QuantumState
{
    public Vector3 stateVector;
    
    

    public QuantumState()
    {
        stateVector = Vector3.zero;
    }

    public QuantumState(Vector3 stateVector)
    {
        this.stateVector = stateVector;
    }
}
