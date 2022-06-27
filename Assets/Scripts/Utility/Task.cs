using System;
[Serializable]
public class Task
{
    //すべて手動。resultは必要な場合のみ使う。
    public int intervalCount { get; set; }
    public bool compleate { get { return compleateGetter(); } }
    public object result { get { return resultGetter(); } }

    Func<bool> compleateGetter;
    Func<object> resultGetter;

    //getterが設定されてないときのみ使われる
    bool _compleateFlag = false;

    //use only when resultGetter is not set
    object _result = false;


    public static Task NULL_TASK = new Task(() => true);
    

    /// <summary>
    /// do nothing if compleateGetter is set
    /// </summary>
    public void SetCompleate()
    {
        _compleateFlag = true;    
    }

    /// <summary>
    /// do nothing if resultGetter is set
    /// </summary>
    /// <param name="result"></param>
    public void SetResult(object result)
    {
        _result = result;
    }

    public Task(Func<bool> compleateGetter = null, Func<object> resultGetter = null)
    {
        if (compleateGetter != null)
        {
            this.compleateGetter = compleateGetter;
        }
        else
        {
            this.compleateGetter = () => _compleateFlag;
        }

        if (resultGetter != null)
        {
            this.resultGetter = resultGetter;
        }
        else
        {
            this.resultGetter = () => _result;
        }
    }

}