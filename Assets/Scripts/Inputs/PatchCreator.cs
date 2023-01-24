using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using MathNet.Numerics.LinearAlgebra.Complex;
public class PatchCreator : InputRequestHandlerComponent
{
    [SerializeField] Vector2[] coords;
    [SerializeField] SerializableDictionary<Vector2, Direction[]> edges;
    [SerializeField] Pauli initializeState;
    Task workingTask = Task.NULL_TASK;
    QuantumPatch nowPatch = null;
    DenseMatrix eigenBase;

    void Start()
    {
        SubmitButton.instance.entity.onClick.AddListener(SubmitInput);
    }

    public override Task RequestInput()
    {
        workingTask = new Task();
        return workingTask;
    }

    void SetPatch(Vector2[] coords, Dictionary<Vector2, Direction[]> smoothEdges)
    {

    }

    void SubmitInput()
    {
        if (!workingTask.compleate)
        {
            eigenBase = initializeState == Pauli.X?QuantumMath.pauliX:QuantumMath.pauliY;

            workingTask.SetResult(new QuantumPatch(coords, edges.GetDictionary(),eigenBase));
            workingTask.SetCompleate();
        }
    }

}