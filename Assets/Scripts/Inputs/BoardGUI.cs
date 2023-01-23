using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using System;

public class BoardGUI : MonoBehaviour
{
    Grid grid;
    [SerializeField] Tilemap board;
    [SerializeField] Tile boardGrid;

    [SerializeField] bool autoUpdate = true;
    //extent.x = pretty extent の時を基準にスケーリングする
    [SerializeField] int prettyExtent = 7;

    private void Awake()
    {
        grid = GetComponentInChildren<Grid>();
    }


    /// <summary>
    /// 今後時間のかかる描画方法をするかもしれないのでTaskを返す
    /// </summary>
    /// <returns></returns>
    [Button]
    public Task RedrawBoard(Vector2Int? extent = null)
    {
        var defaultPosition = board.transform.position;

        ClearBoard();

        if (extent == null)
        {
            extent = BoardManager.instance.nowBoard.extent;
        }
        var scale = Vector3.one * prettyExtent / extent.Value.x;

        transform.localScale = scale;

        for (int x = 0; x < extent.Value.x; x++)
        {
            for (int y = 0; y < extent.Value.y; y++)
            {
                board.SetTile(new Vector3Int(x, y, 0), boardGrid);
            }
        }

        board.transform.position = defaultPosition;
        return Task.NULL_TASK;
    }

    [Button]
    Task ClearBoard()
    {
        board.ClearAllTiles();
        return Task.NULL_TASK;
    }

    /// <summary>
    /// マウスのポジションをタイルパッチ上の座標に変換して取得
    /// </summary>
    /// <returns></returns>
    public Vector3Int GetMouseCoord()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos -= board.transform.position;
        var cellPos = grid.WorldToCell(mousePos);

        return cellPos;
    }

    Task DrawPatch(Patch patch, Vector2Int coordinate, int depth = 1)
    {
        PatchTile patchTile = new PatchTile(patch);

        foreach (var cordPatch in patchTile.patchCellMap)
        {
            Vector3Int cord = (Vector3Int)(coordinate + cordPatch.Key);
            cord.z = depth;
            board.SetTile(cord, cordPatch.Value);
        }

        return Task.NULL_TASK;
    }

    Task RemapEdge()
    {
        for (int x = 0; x < BoardManager.instance.nowBoard.extent.x; x++)
        {
            for (int y = 0; y < BoardManager.instance.nowBoard.extent.y; y++)
            {
                var cord = new Vector3(x, y, 0);
                var cell = BoardManager.instance.nowBoard.GetCell(cord);

                if (cell == null)
                {
                    continue;
                }

                var depth = 4;
                foreach (int direction in Enum.GetValues(typeof(Direction)))
                {
                    cord.z = depth;
                    var property = cell.edges[direction].property as QuantumEdgeProperty;
                    if (property.operatorName == "X")
                    {
                        var edgeTile = EdgeTile.GetTile((Direction)direction);
                        board.SetTile(Vector3Int.FloorToInt(cord), edgeTile);
                    }
                    depth++;
                }
            }
        }

        return Task.NULL_TASK;
    }

    public Task RemapPatch()
    {
        RedrawBoard();
        foreach (var patchVec in BoardManager.instance.nowBoard.patchMap)
        {
            DrawPatch(patchVec.Key, patchVec.Value);
        }
        RemapEdge();

        return Task.NULL_TASK;
    }

    public Task DrawPatchGhost(Patch patch, Vector2Int coordinate)
    {
        RemapPatch();
        //2がゴーストレイヤーであることを前提としている
        DrawPatch(patch, coordinate, 2);

        return Task.NULL_TASK;
    }
}