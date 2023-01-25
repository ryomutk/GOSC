using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using MathNet.Numerics.LinearAlgebra.Complex;
public class PatchCreator : InputRequestHandlerComponent
{
    [SerializeField] Vector2[] coords;
    [SerializeField] SerializableDictionary<Vector2, Direction[]> edges;
    [SerializeField] SerializableDictionary<Button,InitializeType> buttonDict;
    CanvasGroup canvasGroup;
    Task workingTask = Task.NULL_TASK;
    QuantumPatch nowPatch = null;

    void Start()
    {
        foreach(var buttonType in buttonDict)
        {
            buttonType.Key.onClick.AddListener(() => SubmitInput(buttonType.Value));
        }
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        InputRouter.instance.Register(this,InputRequests.patch);
    }

    public override Task RequestInput()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        workingTask = new Task();
        return workingTask;
    }
      

    void SubmitInput(InitializeType arg)
    {
        if (!workingTask.compleate)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            Pauli initialBase = 0;
            if(arg == InitializeType.single0)
            {
                initialBase =  Pauli.Z;
            }
            else if(arg == InitializeType.singleP)
            {
                initialBase = Pauli.X;
            }
            else
            {
                workingTask.SetResult(null);
                workingTask.SetCompleate();
                return;
            }

            workingTask.SetResult(new QuantumPatch(coords, edges.GetDictionary(),initialBase));
            workingTask.SetCompleate();
        }
    }

}