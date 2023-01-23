using UnityEngine;

public class PatchPlacer : InputRequestHandlerComponent
{
    [SerializeField] Vector2Int defaultCoords;
    Vector2 nowCoord;
    Task workingTask = Task.NULL_TASK;


    void Start()
    {
        SubmitButton.instance.entity.onClick.AddListener(() => SubmitInput());
    }


    public override Task RequestInput()
    {
        workingTask = new Task();
        nowCoord = defaultCoords;
        workingTask.SetResult(nowCoord);
        workingTask.intervalCount++;
        return workingTask;
    }

    void Update()
    {
        if (!workingTask.compleate)
        {
            MoveInput();
        }
    }

    void SubmitInput()
    {
        if (!workingTask.compleate)
        {
            Patch patch = BoardManager.instance.prepairedPatch;
            Board boardData = BoardManager.instance.nowBoard;
            if (boardData.Register(nowCoord, patch))
            {
                workingTask.intervalCount += 1;
                workingTask.SetCompleate();
            }
        }
    }


    bool moved = false;
    void MoveInput()
    {

        var x = Input.GetKeyDown(KeyCode.RightArrow)?1:Input.GetKeyDown(KeyCode.LeftArrow)?-1:0;
        var y = Input.GetKeyDown(KeyCode.UpArrow)?1:Input.GetKeyDown(KeyCode.DownArrow)?-1:0;
        
        #if DEBUG
        //Debug.Log("arrow input x:"+x+" y:" + y);
        #endif

        if ((x != 0 || y != 0) && !moved)
        {
            moved = true;
            nowCoord += new Vector2(x, y);
            workingTask.intervalCount++;
            workingTask.SetResult(nowCoord);
        }
        else if (moved)
        {
            if (x == 0 && y == 0)
            {
                moved = false;
            }
        }


    }
}