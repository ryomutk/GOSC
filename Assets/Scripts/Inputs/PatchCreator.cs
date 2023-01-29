using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using MathNet.Numerics.LinearAlgebra.Complex;
public class PatchCreator : InputRequestHandlerComponent
{
    [SerializeField] Vector2[] coords;
    [SerializeField] SerializableDictionary<Button, InitializeType> buttonDict;
    [SerializeField] EdgeSelector edgeSelector;
    CanvasGroup canvasGroup;
    Task workingTask = Task.NULL_TASK;
    QuantumPatch nowPatch = null;

    void Start()
    {
        foreach (var buttonType in buttonDict)
        {
            buttonType.Key.onClick.AddListener(() => SubmitInput(buttonType.Value));
        }
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        InputRouter.instance.Register(this, InputRequests.patch);
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
            bool inv = false;
            if (arg == InitializeType.single0)
            {
                initialBase = Pauli.Z;
            }
            else if (arg == InitializeType.singleP)
            {
                initialBase = Pauli.X;
            }
            else if (arg == InitializeType.single0Inv)
            {
                inv = true;
                initialBase = Pauli.Z;
            }
            else if (arg == InitializeType.singlePInv)
            {
                inv = true;
                initialBase = Pauli.X;
            }


            var edges = new Dictionary<Vector2, Direction[]>();
            foreach (var cord in coords)
            {
                edges[cord] = edgeSelector.GetSmooths();
            }
            workingTask.SetResult(new QuantumPatch(coords, edges, initialBase));
            workingTask.SetCompleate();
        }
    }

}