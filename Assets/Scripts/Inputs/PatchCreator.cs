using UnityEngine;
using UnityEngine.UI;

public class PatchCreator : InputRequestHandlerComponent
{
    [SerializeField] Vector2[] coords;
    Task workingTask = Task.NULL_TASK;

    void Start()
    {
        SubmitButton.instance.entity.onClick.AddListener(SubmitInput);    
    }

    public override Task RequestInput()
    {
        workingTask = new Task();
        return workingTask;
    }

    void SubmitInput()
    {
        if (!workingTask.compleate)
        {
            workingTask.SetResult(new Patch(coords));
            workingTask.SetCompleate();
        }
    }

}