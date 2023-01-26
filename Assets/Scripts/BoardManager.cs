using System.Collections;
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
public class BoardManager : Singleton<BoardManager>
{
    [ShowInInspector, ReadOnly] public Board nowBoard { get; private set; }
    public QuantumPatch prepairedPatch { get; private set; }
    public SessionState state { get; private set; }
    [SerializeField] BoardGUI board;
    [SerializeField] SerializableDictionary<Pauli, Button> baseSelectors;
    Pauli _pauliMode = Pauli.X;
    public Pauli pauliMode { get { return _pauliMode; } }

    private void Start()
    {
        baseSelectors[Pauli.X].onClick.AddListener(() =>
        {
            baseSelectors[Pauli.X].interactable = false;
            baseSelectors[Pauli.Z].interactable = true;
            _pauliMode = Pauli.X;
            board.RemapPatch();
        });

        baseSelectors[Pauli.X].interactable = false;

        baseSelectors[Pauli.Z].onClick.AddListener(() =>
        {
            baseSelectors[Pauli.Z].interactable = false;
            baseSelectors[Pauli.X].interactable = true;
            _pauliMode = Pauli.Z;
            board.RemapPatch();
        });

        state = SessionState.disabled;
        StartCoroutine(SessionLoop());
    }

    /// <summary>
    /// make new board。
    /// </summary>
    /// <param name="extent">size of board</param>
    /// <returns>board prepair task</returns>
    void RequestNewBoard()
    {
        state = SessionState.prepairing;
        var boardPrepare = InputRouter.instance.RequestInput(InputRequests.newBoard);

        StartCoroutine(WaitForBoardPrepare(boardPrepare));
    }

    IEnumerator WaitForBoardPrepare(Task boardPrepareTask)
    {
        yield return new WaitUntil(() => boardPrepareTask.compleate);

        nowBoard = boardPrepareTask.result as Board;
        board.RedrawBoard();
        var out_Task = OutputRouter.instance.RequestOutput(OutputRequests.info, new String[1] { "Waiting for Action" });

        yield return new WaitUntil(() => out_Task.compleate);
        state = SessionState.actionSelect;
    }

    IEnumerator SessionLoop()
    {
        //一番最初はボードを作ってもらう   
        RequestNewBoard();


        //state == actionSelectになるたびに繰り返されるループ
        while (true)
        {
            yield return new WaitUntil(() => state == SessionState.actionSelect);

            var actionSelect = InputRouter.instance.RequestInput(InputRequests.actionSelect);

            yield return new WaitUntil(() => actionSelect.compleate);

            var selectedAction = (BoardActions)actionSelect.result;
            switch (selectedAction)
            {
                case BoardActions.startNew:
                    RequestNewBoard();
                    break;
                case BoardActions.placePatch:
                    yield return StartCoroutine(PlaceLoop());
                    break;
                case BoardActions.extendPatch:
                    throw new NotImplementedException();
                case BoardActions.measurePatch:
                    if(nowBoard.patchMap.Count == 0)
                    {
                        OutputRouter.instance.RequestOutput(OutputRequests.info,"Please place at least one patch");
                        break;
                    }

                    yield return StartCoroutine(MeasureLoop());
                    break;
            }

        }

    }

    IEnumerator MeasureLoop()
    {
        state = SessionState.patchInput;
        var measureRequest = InputRouter.instance.RequestInput(InputRequests.selectMeasurePatch);

        while (!measureRequest.compleate)
        {
            yield return new WaitUntil(() => measureRequest.result != null);

            state = SessionState.patchMeasure;
            var last = measureRequest.result as Dictionary<QuantumPatch, bool>;

            foreach (var patchBool in last)
            {
                nowBoard.PatchSelect(patchBool.Key, !patchBool.Value);
            }
            board.RemapSelected();
        }

        var selecteds = (measureRequest.result as Dictionary<QuantumPatch, bool>).Where(x => x.Value).Select(x => x.Key).ToArray();
        if (selecteds.Length == 1)
        {
            Pauli selected = pauliMode;
            var result = QuantumMath.Measure(selecteds[0], selected == Pauli.X ? QuantumMath.pauliX : QuantumMath.pauliZ);
            nowBoard.RemovePatch(selecteds[0]);
            OutputRouter.instance.RequestOutput(OutputRequests.info, result.infoMsg);
        }
        else if (selecteds.Length == 2)
        {
            var ifTangent = nowBoard.GetTangentEdges(selecteds[0], selecteds[1]);
            if (ifTangent.Count() == 2)
            {
                EntangleResult result = QuantumMath.Measure(selecteds[0], selecteds[1], ifTangent[selecteds[0]], ifTangent[selecteds[1]]);
                OutputRouter.instance.RequestOutput(OutputRequests.info, result.info);
            }
        }
        else if (selecteds.Length > 2)
        {
            OutputRouter.instance.RequestOutput(OutputRequests.info, "Please select one || two patches next to each other");
        }
        else
        {

        }

        nowBoard.DeselectAll();
        board.RemapPatch();

        state = SessionState.actionSelect;
    }

    IEnumerator PlaceLoop()
    {
        state = SessionState.patchInput;
        var patchRequest = InputRouter.instance.RequestInput(InputRequests.patch);
        yield return new WaitUntil(predicate: () => patchRequest.compleate);

        if (patchRequest.result == null)
        {
            state = SessionState.actionSelect;
            yield break;
        }

        prepairedPatch = patchRequest.result as QuantumPatch;
        state = SessionState.patchPlace;

        var patchPlace = InputRouter.instance.RequestInput(InputRequests.patchPlace);
        var count = 0;
        while (!patchPlace.compleate)
        {
            yield return new WaitWhile(() => patchPlace.intervalCount == count);

            if (patchPlace.compleate)
            {
                break;
            }

#if DEBUG
            Debug.Log("Patch Place Updated:" + (Vector2)patchPlace.result);
#endif

            count = patchPlace.intervalCount;
            var nowCoord = (Vector2)patchPlace.result;
            nowCoord.x = Mathf.Clamp(nowCoord.x, 0,nowBoard.extent.x);
            nowCoord.y = Mathf.Clamp(nowCoord.y, 0,nowBoard.extent.y);


            board.DrawPatchGhost(BoardManager.instance.prepairedPatch, Vector2Int.RoundToInt(nowCoord));
        }
        board.RemapPatch();
        state = SessionState.actionSelect;
    }

}