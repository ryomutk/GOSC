using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class UIRouter:Singleton<UIRouter>
{
    [SerializeField] SerializableDictionary<InputRequests,InputRequestHandlerComponent> inputHandlers;

    public Task RequestInput(InputRequests requestName)
    {
        InputRequestHandler selectedInput = inputHandlers[requestName];

        return selectedInput.RequestInput();
    }

}