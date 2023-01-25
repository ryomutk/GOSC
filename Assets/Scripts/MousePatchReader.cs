using UnityEngine;
using System.Collections.Generic;
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



    private void Start()
    {
        boardGUI = GetComponent<BoardGUI>();
        InputRouter.instance.Register(this, InputRequests.selectMeasurePatch);
        SubmitButton.instance.entity.onClick.AddListener(() => SubmitInput());
    }

    public override Task RequestInput()
    {
        SubmitButton.instance.text.text = "Measure";
        processingTask = new Task();

        return processingTask;
    }

    void SubmitInput()
    {
        if (processingTask != null)
        {
            processingTask.SetCompleate();
            processingTask = null;
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
                var msg = patch.getStateInfo(QuantumMath.pauliZ);

                OutputRouter.instance.RequestOutput(OutputRequests.info, msg);
            }
            else
            {
                onpatch = false;
                nowPatch = null;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (selecteds.ContainsKey(patch))
                {

                    selecteds[patch] = !selecteds[patch];
                }
                else
                {
                    selecteds[patch] = true;
                }
            }
        }
    }


}