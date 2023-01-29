using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HomePanel:InputRequestHandlerComponent
{
    [SerializeField] Button patchSelectButton;
    [SerializeField] Button newBoardButton;
    [SerializeField] Button mesureButton;
    [SerializeField] Button deformButton;
    List<Task> waitingTasks = new List<Task>();

    void Start()
    {
        patchSelectButton.onClick.AddListener(()=>ButtonPush(BoardActions.placePatch));
        newBoardButton.onClick.AddListener(()=>ButtonPush(BoardActions.startNew));
        mesureButton.onClick.AddListener(()=>ButtonPush(BoardActions.measurePatch));
        deformButton.onClick.AddListener(()=>ButtonPush(BoardActions.extendPatch));
        InputRouter.instance.Register(this,InputRequests.actionSelect);
    }

    public override Task RequestInput()
    {
        var task = new Task();
        waitingTasks.Add(task);

        return task;
    }

    void ButtonPush(BoardActions boardAction)
    {
        foreach(var task in waitingTasks)
        {
            #if DEBUG
            Debug.Log(boardAction + " selected");
            #endif
            task.SetResult(boardAction);
            task.SetCompleate();
        }

        waitingTasks.Clear();
    }

}