using UnityEngine;
using System.Collections.Generic;

public class ScriptableLoader : MonoBehaviour {
    [SerializeField] List<ScriptableObject> scriptables;
    private void Start() {
        foreach(var s in scriptables)
        {
            if(s is EdgeTile e)
            {
                e.load();
            }
            else if(s is PatchCellTile p)
            {
                p.load();
            }
        }
    }
}
