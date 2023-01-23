using UnityEngine;
using TMPro;
using System.Collections;

public class TextOverlay : OutputRequestHandlerComponent
{
    TMP_Text textarea;
    Coroutine nowRoutine=null;
    TemporaryTextArgs nowArgs=null;
    Task nowTask = null;
    private void Start()
    {
        textarea = GetComponentInChildren<TMP_Text>();
        OutputRouter.instance.RegisterOutput(OutputRequests.overlay,this);
    }

    public override Task RequestOutput(object args)
    {
        var arg = args as TemporaryTextArgs;
        if(nowRoutine==null)
        {
            arg.lastMessage = textarea.text;
        }
        else
        {
            arg.lastMessage = nowArgs.lastMessage;
            StopCoroutine(nowRoutine);
            nowTask.SetCompleate();
        }
        nowRoutine = StartCoroutine(OverlayTask(arg));

        nowTask = new Task();
        
        return nowTask;
    }

    IEnumerator OverlayTask(TemporaryTextArgs args)
    {
        textarea.text = args.message;
        yield return new WaitUntil(args.checkCompleate);    
        textarea.text = args.lastMessage;
        nowTask.SetCompleate();

        nowTask = null;
        nowArgs = null;
        nowRoutine = null;
    }
    
}