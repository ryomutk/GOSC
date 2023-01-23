using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InfoTextbox : OutputRequestHandlerComponent
{
    ScrollRect popup;
    TMP_Text textArea;


    private void Start()
    {
        popup = GetComponentInChildren<ScrollRect>();
        textArea = GetComponentInChildren<TMP_Text>();
        OutputRouter.instance.RegisterOutput(OutputRequests.info,this);
    }

    public override Task RequestOutput(object args)
    {
        var inputLines = args as string;
        textArea.text = inputLines;

        return Task.NULL_TASK;
    }


}
