using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

public class InputRouter : Singleton<InputRouter>
{
    [SerializeField] SerializableDictionary<InputRequests, InputRequestHandlerComponent> inputHandlers;
    [ShowInInspector]Dictionary<InputRequests, Task> waitingTasks = new Dictionary<InputRequests, Task>();

    /// <summary>
    /// input を Requestし、タスクを受け取る
    /// 非推奨
    /// </summary>
    /// <param name="requestName"></param>
    /// <returns></returns>
    public Task RequestInput(InputRequests requestName)
    {
        InputRequestHandler selectedInput = inputHandlers[requestName];
        var task = selectedInput.RequestInput();
        waitingTasks[requestName] = task;
        return task;
    }

    void Update()
    {
        UpdateInput();
    }

    List<InputRequests> removeTasks = new List<InputRequests>();
    void UpdateInput()
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



    /// <summary>
    /// Read input.
    /// Use this like other input.Getxx()
    /// ただし、inputの結果としてnullが帰る場合は終了が感知できないので注意。
    /// In those cases use Request Input otherwise.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>inputResult,null if </returns>
    public object GetInput(InputRequests request)
    {
        if (waitingTasks.TryGetValue(request, out Task task))
        {
            if (task.compleate)
            {
                return task.result;
            }
            else
            {
                return null;
            }
        }

        waitingTasks[request] = RequestInput(request);

        return null;
    }
}