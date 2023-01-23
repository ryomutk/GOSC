using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

/// <summary>
/// タイルのSpriteを保持しているScriptableObject（Tile(Unity Engine)）
/// </summary>
[CreateAssetMenu(menuName = "Tile/EdgeTile")]
public class EdgeTile : Tile
{
    [ShowInInspector] static Dictionary<Direction, EdgeTile> tileDict = new Dictionary<Direction, EdgeTile>();
    [SerializeField] Direction direction;

    private void OnValidate()
    {
        while (tileDict.Any(x => x.Value == this))
        {
            tileDict.Remove(tileDict.First(x => x.Value == this).Key);
        }
        if (!tileDict.ContainsKey(direction))
        {
            tileDict[direction] = this;
        }
    }


    public static EdgeTile GetTile(Direction shape)
    {
        #if DEBUG
        Debug.Log(shape+"prepaired");
        #endif
        return tileDict[shape];
    }
}