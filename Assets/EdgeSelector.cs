using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EdgeSelector : MonoBehaviour
{
    Toggle[] toggles;
    Dictionary<Direction,bool> selecteds = new Dictionary<Direction, bool>();

    private void Awake() {
        toggles = GetComponentsInChildren<Toggle>();
        for(var i = 0; i <toggles.Length; i++)
        {
            selecteds[(Direction)i] = toggles[i].isOn;
            var target = i;
            toggles[i].onValueChanged.AddListener((x)=>ToggleDirection((Direction)target,x));
        }
    }
    void ToggleDirection(Direction direction,bool state)
    {
        selecteds[direction] = state;
        Debug.Log(direction);
    }
    public Direction[] GetSmooths()
    {
        return selecteds.Where(x=>x.Value == true).Select(x=>x.Key).ToArray();
    }

}
