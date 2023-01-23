using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BoardCreator:InputRequestHandlerComponent
{
    [SerializeField] TMP_Dropdown x_input;
    [SerializeField] TMP_Dropdown y_input;
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
        Vector2 extent = new Vector2(int.Parse(x_input.options[x_input.value].text),int.Parse(y_input.options[y_input.value].text));
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