using System;
using UnityEngine;

/// <summary>
/// 一時的に表示するテキストの引数。
/// 
/// </summary>
[Serializable]
public class TemporaryTextArgs
{
    float limitTime = -1;
    Func<bool> endFlag;
    public string message;
    public string lastMessage
    {
        get;set;
    }

    public TemporaryTextArgs(int limitTime,string message)
    {
        this.message = message;
        this.limitTime = limitTime;
    }

    public TemporaryTextArgs(Func<bool> endFlag,string message)
    {
        this.endFlag = endFlag;
        this.message = message;
    }

    public bool checkCompleate()
    {
        if(limitTime==-1)
        {
            return endFlag();
        }
        else
        {
            limitTime -= Time.deltaTime;

            return limitTime < 0;
        }
    }
}