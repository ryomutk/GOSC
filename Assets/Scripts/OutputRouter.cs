using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

public class OutputRouter : Singleton<OutputRouter>
{
    SerializableDictionary<OutputRequests, OutputRequestHandlerComponent> outputHandlers = new SerializableDictionary<OutputRequests, OutputRequestHandlerComponent>();
    [ShowInInspector]Dictionary<OutputRequests, Task> waitingTasks = new Dictionary<OutputRequests, Task>();

    /// <summary>
    /// output を Requestし、タスクを受け取る
    /// </summary>
    /// <param name="requestName"></param>
    /// <returns></returns>
    public Task RequestOutput(OutputRequests requestName,object args)
    {
        OutputRequestHandler selectedOutput = outputHandlers[requestName];
        var task = selectedOutput.RequestOutput(args);
        waitingTasks[requestName] = task;
        return task;
    }

    public void RegisterOutput(OutputRequests requestname,OutputRequestHandlerComponent handler)
    {
        if(outputHandlers.keys.Contains(requestname))
        {
            #if DEBUG
            Debug.Log("Output:"+requestname+" Overwritten");
            #endif
        }
        outputHandlers[requestname] = handler;
    }

    void Update()
    {
        UpdateOutput();
    }

    List<OutputRequests> removeTasks = new List<OutputRequests>();
    void UpdateOutput()
    {
        foreach (var reqtask in waitingTasks)
        {
            if (reqtask.Value.intervalCount > 0)
            {
                removeTasks.Add(reqtask.Key);
            }
            else if (reqtask.Value.compleate)
            {
                reqtask.Value.intervalCount += 1;
            }
        }

        foreach (var task in removeTasks)
        {
            waitingTasks.Remove(task);
        }
    }

}