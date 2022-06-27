using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BoardCreator:InputRequestHandlerComponent
{
    [SerializeField] TMP_InputField inputField;
    List<Task> waitingTasks = new List<Task>();

    void Start()
    {
        SubmitButton.instance.entity.onClick.AddListener(ButtonPush);
    }

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
        Debug.Log(extent);
        var board = new Board(extent);

        foreach(var task in waitingTasks)
        {
            task.SetResult(board);
            task.SetCompleate();
        }

        waitingTasks.Clear();
    }
}