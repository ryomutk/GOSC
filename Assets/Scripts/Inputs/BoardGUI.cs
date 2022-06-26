using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BoardGUI : MonoBehaviour
{
    Tilemap board;
    Tile boardGrid;     
    PatchTile patchTile;
    [SerializeField] bool autoUpdate = true;

    /// <summary>
    /// 今後時間のかかる描画方法をするかもしれないのでTaskを返す
    /// </summary>
    /// <returns></returns>
    Task RedrawBoard()
    {
        var extent = BoardManager.instance.nowBoard.extent;
        for (int x = 0; x < extent.x; x++)
        {
            for (int y = 0; y < extent.y; y++)
            {

                board.SetTile(new Vector3Int(x,y,0),boardGrid);
            }
        }

        return Task.NULL_TASK;
    }

    Task DrawPatch(Patch patch,Vector2Int coordinate)
    {
        patchTile.Load(patch);
        board.SetTile(new Vector3Int(coordinate.x,coordinate.y,1),patchTile);

        return Task.NULL_TASK;
    }

    public Task DrawPatchGhost(Patch patch,Vector2Int coordinate,Color color)
    {
        board.ClearAllTiles();
        RedrawBoard();
        foreach(var patchVec in BoardManager.instance.nowBoard.patchMap)
        {
            DrawPatch(patchVec.Key,patchVec.Value);
        }

        Color transparen = color;
        transparen.a = 0.5f;
        patchTile.color = transparen;

        DrawPatch(patch,coordinate);
        patchTile.color = Color.white;

        return Task.NULL_TASK;
    }
}