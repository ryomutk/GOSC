using UnityEngine;

public class AbstractShape
{
    public Vector2 direction =Vector2.zero;
    public int connectCount;
    //Vector2.zero || 2 の時は判別できないから仕方ない
    public bool isTopDown = false;

    public override string ToString()
    {
        return base.ToString() + ": direction="+ direction+" connects="+connectCount;
    }
}