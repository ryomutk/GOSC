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
    [SerializeField] Tile hilightGrid;
    [SerializeField] bool autoUpdate = true;
    //extent.x = pretty extent の時を基準にスケーリングする
    [SerializeField] int prettyExtent = 7;
    [SerializeField] int hilightDepth = 20;
    [SerializeField] TMPro.TMP_Text labelText;
    TMPro.TMP_Text[,] texts;
    RectTransform canvasTransform;

    Board nowboard { get { return BoardManager.instance.nowBoard; } }

    private void Awake()
    {
        canvasTransform = GetComponentInChildren<RectTransform>();
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

        texts = new TMPro.TMP_Text[extent.Value.x, extent.Value.y];


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

        for (int x = 0; x < extent.Value.x; x++)
        {
            for (int y = 0; y < extent.Value.y; y++)
            {
                var world_pos = board.CellToWorld(new Vector3Int(x, y));

                var i = Instantiate(labelText);
                i.transform.SetParent(canvasTransform, false);
                i.transform.position = (Vector2)(world_pos + boardGrid.sprite.bounds.extents * 8 / 5);
                texts[x, y] = i;
            }
        }
        return Task.NULL_TASK;
    }

    public Task RemapSelected()
    {
        var extent = BoardManager.instance.nowBoard.extent;
        for (int x = 0; x < extent.x; x++)
        {
            for (int y = 0; y < extent.y; y++)
            {
                var light =  board.GetTile(new Vector3Int(x,y,hilightDepth));
                if (nowboard.selected[x, y] &&light ==null)
                {
                    board.SetTile(new Vector3Int(x, y, hilightDepth), hilightGrid);
                }
                else if( light !=null && !nowboard.selected[x,y])
                {
                    board.SetTile(new Vector3Int(x, y, hilightDepth), null);
                }
            }
        }

        return Task.NULL_TASK;
    }

    [Button]
    Task ClearBoard()
    {
        board.ClearAllTiles();
        DisposeTexts();
        return Task.NULL_TASK;
    }

    void DisposeTexts()
    {
        if (texts != null)
        {
            foreach (var tex in texts)
            {
                Destroy(tex);
            }
        }

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

        texts[coordinate.x, coordinate.y].text = (patch as QuantumPatch).getStateInfo(QuantumMath.GetPauliGate(BoardManager.instance.pauliMode));

        return Task.NULL_TASK;
    }

    Task RemapEdge()
    {
        for (int x = 0; x < BoardManager.instance.nowBoard.extent.x; x++)
        {
            for (int y = 0; y < BoardManager.instance.nowBoard.extent.y; y++)
            {
                var cord = new Vector3(x, y, 1);
                var cell = BoardManager.instance.nowBoard.GetCell(cord);

                var depth = 10;
                if (cell == null)
                {
                    cord.z = depth;
                    board.SetTile(Vector3Int.FloorToInt(cord),null); 
                    continue;
                }

                foreach (int direction in Enum.GetValues(typeof(Direction)))
                {
                    cord.z = depth;
                    var property = cell.edges[direction].property as QuantumEdgeProperty;
                    if (property.operatorType == Pauli.X)
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
        RemapSelected();

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