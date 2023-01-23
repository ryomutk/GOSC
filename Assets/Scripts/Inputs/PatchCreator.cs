using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PatchCreator : InputRequestHandlerComponent
{
    [SerializeField] Vector2[] coords;
    [SerializeField] SerializableDictionary<Vector2, Direction[]> edges;
    Task workingTask = Task.NULL_TASK;
    QuantumPatch nowPatch = null;

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

            workingTask.SetResult(new QuantumPatch(coords, edges.GetDictionary()));
            workingTask.SetCompleate();
        }
    }

}