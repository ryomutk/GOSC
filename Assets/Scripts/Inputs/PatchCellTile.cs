using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

/// <summary>
/// タイルのSpriteを保持しているScriptableObject（Tile(Unity Engine)）
/// </summary>
[CreateAssetMenu(menuName = "Tile/Patchtile")]
public class PatchCellTile : Tile
{
    [ShowInInspector] static Dictionary<PatchShape, PatchCellTile> tileDict = new Dictionary<PatchShape, PatchCellTile>();
    [SerializeField] PatchShape shape;
    [SerializeField] int rotation;
    [SerializeField] int ghostLayer = 2;
    [SerializeField] Color ghostColor = Color.red;

    private void OnValidate()
    {
        while (tileDict.Any(x => x.Value == this))
        {
            tileDict.Remove(tileDict.First(x => x.Value == this).Key);
        }
        if (!tileDict.ContainsKey(shape))
        {
            tileDict[shape] = this;
        }
        var rot = Quaternion.Euler(0, 0, rotation);
        if (transform.rotation != rot)
        {
            //まずは0に戻す
            transform *= Matrix4x4.Rotate(Quaternion.Inverse(transform.rotation));

            //回転を加える
            transform *= Matrix4x4.Rotate(rot);
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        if (position.z == ghostLayer)
        {
            tileData.color = ghostColor;
        }
    }


    public static PatchCellTile GetTile(PatchShape shape)
    {
        #if DEBUG
        Debug.Log(shape+"prepaired");
        #endif
        return tileDict[shape];
    }

    public static PatchCellTile GetTile(AbstractShape shape)
    {
        if (shape.connectCount == 0)
        {
            return tileDict[PatchShape.single];
        }
        else if (shape.connectCount == 3 || shape.connectCount == 1)
        {
            var cc = shape.connectCount;
            if (shape.direction == Vector2.down)
            {
                return cc == 1 ? GetTile(PatchShape.down) : GetTile(PatchShape.top_else);
            }
            else if (shape.direction == Vector2.left)
            {
                return cc == 1 ? GetTile(PatchShape.left) : GetTile(PatchShape.right_else);
            }
            else if (shape.direction == Vector2.right)
            {
                return cc == 1 ? GetTile(PatchShape.right) : GetTile(PatchShape.left_else);
            }
            else
            {
                return cc == 1 ? GetTile(PatchShape.up) : GetTile(PatchShape.bottom_else);
            }
        }
        else if (shape.connectCount == 2)
        {
            if (shape.direction == Vector2.zero)
            {
                if (shape.isTopDown)
                {
                    return GetTile(PatchShape.top_down);
                }
                else
                {
                    return GetTile(PatchShape.right_left);
                }
            }
            else
            {
                var sum = shape.direction.x + shape.direction.y;
                if (sum == 2)
                {
                    return GetTile(PatchShape.up_right);
                }
                else if (sum == -2)
                {
                    return GetTile(PatchShape.left_down);
                }
                else if (sum == 0)
                {
                    if (shape.direction.x == 1)
                    {
                        return GetTile(PatchShape.right_down);
                    }
                    else if (shape.direction.x == -1)
                    {
                        return GetTile(PatchShape.left_up);
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
                else
                {
                    throw new System.Exception();
                }
            }
        }
        else if (shape.connectCount == 4)
        {
            return GetTile(PatchShape.full);
        }
        else
        {
            throw new System.Exception();
        }
    }

}