using UnityEngine;
using System.Collections;
using System;

public class BoardManager : Singleton<BoardManager>
{
    public Board nowBoard { get; private set; }
    public Patch prepairedPatch { get; private set; }
    public SessionState state = SessionState.disabled;


    /// <summary>
    /// make new board。
    /// </summary>
    /// <param name="extent">size of board</param>
    /// <returns>board prepair task</returns>
    void RequestNewBoard()
    {
        state = SessionState.prepairing;
        var boardPrepare = UIRouter.instance.RequestInput(InputRequests.newBoard);
        
        StartCoroutine(WaitForBoardPrepare(boardPrepare));
    }

    IEnumerator WaitForBoardPrepare(Task boardPrepareTask)
    {
        yield return new WaitUntil(() => boardPrepareTask.compleate);
        nowBoard = boardPrepareTask.result as Board;

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

            var actionSelect = UIRouter.instance.RequestInput(InputRequests.actionSelect);

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
            }

        }

    }

    IEnumerator PlaceLoop()
    {
        state = SessionState.patchInput;
        var patchRequest = UIRouter.instance.RequestInput(InputRequests.patch);
        yield return new WaitUntil(() => patchRequest.compleate);

        if (patchRequest.result == null)
        {
            state = SessionState.actionSelect;
            yield break;
        }

        prepairedPatch = patchRequest.result as Patch;
        state = SessionState.patchPlace;

        var patchPlace = UIRouter.instance.RequestInput(InputRequests.patchPlace);
        yield return new WaitUntil(() => patchPlace.compleate);
        state = SessionState.actionSelect;
    }

}