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
    static float root2bunnoichi = 0.707106781186547f;
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
                //OutputRouter.instance.RequestOutput(OutputRequests.info, patch.GetInfo());
                var result = patch.StateInBase(QuantumMath.pauliZ);
                var msg = "";
                var count = 0;
                foreach (var cv in result)
                {
                    if (count == 0)
                    {
                        msg += (cv.Key.Real != 0 ? cv.Key.Real+(cv.Key.Imaginary<0?" + ":" - "):"") + (cv.Key.Imaginary != 0?cv.Key.Imaginary+" + ":"" )+ (QuantumMath.GetStateKet(cv.Value, "|a〉"));
                    }
                    else
                    {
                        msg += (cv.Key.Real != 0 ? (cv.Key.Real < 0?" ":" + ")+ cv.Key.Real + (cv.Key.Imaginary<0?" + ":" - "):"") + (cv.Key.Imaginary != 0?cv.Key.Imaginary+" + ":"" )+ (QuantumMath.GetStateKet(cv.Value, "|a〉<sup>T</sup>"));
                    }

                    count++;
                }

                OutputRouter.instance.RequestOutput(OutputRequests.info,msg);
            }
            else
            {
                onpatch = false;
                nowPatch = null;
            }
        }
    }
}