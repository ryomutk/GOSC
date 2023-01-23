using UnityEngine;

public class MousePatchReader : MonoBehaviour
{
    BoardGUI boardGUI;
    Board nowBoard
    {
        get { return BoardManager.instance.nowBoard; }
    }
    QuantumPatch nowPatch;
    TemporaryTextArgs nowText = null;
    bool onpatch = false;
    private void Start()
    {
        boardGUI = GetComponent<BoardGUI>();
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
                OutputRouter.instance.RequestOutput(OutputRequests.info, patch.GetInfo());
            }
        }
        else
        {
            onpatch = false;
            nowPatch = null;
        }
    }
}