using UnityEngine;
using System.Collections.Generic;

public class PatchTile
{
    public Dictionary<Vector2Int,PatchCellTile> patchCellMap = new Dictionary<Vector2Int, PatchCellTile>();
    public Patch entity;
    public PatchTile(Patch patch)
    {
        Load(patch);
    }

    public bool Load(Patch patch)
    {
        entity = patch;
        foreach(var cordcell in patch.cellMap)
        {
            patchCellMap[Vector2Int.RoundToInt(cordcell.Key)] = PatchCellTile.GetTile(entity.cellShapes[cordcell.Value]);
        }

        return true;
    }
}
