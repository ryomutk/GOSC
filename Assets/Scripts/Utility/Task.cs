using System;

public class Task
{
    //すべて手動。resultは必要な場合のみ使う。
    public int intervalCount{get;set;}
    public bool compleate{get {return compleateGetter();}}
    public object result{get{return resultGetter();}}

    Func<bool> compleateGetter;
    Func<object> resultGetter;
    public bool hasResult{get;private set;}

    public static Task NULL_TASK = new Task(() => true);

    public Task(Func<bool> compleateGetter,Func<object> resultGetter = null)
    {
        this.compleateGetter = compleateGetter;
        if(resultGetter != null)
        {
            hasResult = true;
            this.resultGetter = resultGetter;
        }
        hasResult = false;
    }

}