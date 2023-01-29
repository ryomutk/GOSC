using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class MousePatchReader : InputRequestHandlerComponent
{
    BoardGUI boardGUI;
    Board nowBoard
    {
        get { return BoardManager.instance.nowBoard; }
    }
    QuantumPatch nowPatch;
    TemporaryTextArgs nowText = null;
    bool onpatch = false;
    Task processingTask = null;
    Dictionary<QuantumPatch, bool> selecteds = new Dictionary<QuantumPatch, bool>();
    Dictionary<Vector2Int, bool> selectedCoords = new Dictionary<Vector2Int, bool>();
    SelectMode selectMode = 0;



    private void Start()
    {
        boardGUI = GetComponent<BoardGUI>();
        InputRouter.instance.Register(this, InputRequests.selectMeasurePatch);
        InputRouter.instance.Register(this, InputRequests.reformPatch);
        SubmitButton.instance.entity.onClick.AddListener(() => SubmitInput());
    }

    public override Task RequestInput()
    {
        if (BoardManager.instance.state == SessionState.patchReform)
        {
            selectMode = SelectMode.cell;
        }
        else
        {
            selectMode = SelectMode.patch;
        }

        processingTask = new Task();

        return processingTask;
    }

    void SubmitInput()
    {
        if (processingTask != null)
        {
            processingTask.SetCompleate();
            processingTask = null;
            selecteds = new Dictionary<QuantumPatch, bool>();
            selectedCoords = new Dictionary<Vector2Int, bool>();
        }
    }

    private void Update()
    {
        Vector3Int coord = boardGUI.GetMouseCoord();
        if (nowBoard != null && nowBoard.Check(((Vector2Int)coord)))
        {
            var patch = nowBoard.Get(((Vector2Int)coord)) as QuantumPatch;
            if (patch != nowPatch)
            {
                nowPatch = patch;
                //OutputRouter.instance.RequestOutput(OutputRequests.info, patch.GetInfo());
                var msg = patch.getStateInfo(QuantumMath.GetPauliGate(BoardManager.instance.pauliMode));

                OutputRouter.instance.RequestOutput(OutputRequests.info, msg);
            }
            else
            {
                onpatch = false;
                nowPatch = null;
            }

            if (processingTask != null && Input.GetMouseButtonDown(0))
            {
                if (selectMode == SelectMode.patch)
                {
                    if (selecteds.ContainsKey(patch))
                    {

                        selecteds[patch] = !selecteds[patch];
                    }
                    else
                    {
                        selecteds[patch] = true;
                    }
                    processingTask.SetResult(selecteds);

                }
            }
        }

        if (selectMode==SelectMode.cell&&processingTask != null && Input.GetMouseButtonDown(0))
        {
            if (coord.x >= BoardManager.instance.nowBoard.extent.x ||coord.y >= BoardManager.instance.nowBoard.extent.y)
            {
                return;
            }

            if (selectedCoords.ContainsKey((Vector2Int)coord))
            {
                selectedCoords[(Vector2Int)coord] = !selectedCoords[(Vector2Int)coord];
            }
            else
            {
                selectedCoords[(Vector2Int)coord] = true;
            }

            processingTask.SetResult(selectedCoords);
        }

    }


}