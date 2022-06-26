using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class BoardCreator:InputRequestHandlerComponent
{
    [SerializeField] Button submitButton;
    [SerializeField] InputField inputField;
    List<Task> waitingTasks = new List<Task>();

    public override Task RequestInput()
    {
        var task = new Task();
        waitingTasks.Add(task);

        return task;
    }

    void ButtonPush()
    {
        var txt = inputField.text.Split();
        Vector2 extent = new Vector2(int.Parse(txt[0]),int.Parse(txt[1]));
        var board = new Board(extent);

        foreach(var task in waitingTasks)
        {
            task.SetResult(board);
            task.SetCompleate();
        }

        waitingTasks.Clear();
    }
}