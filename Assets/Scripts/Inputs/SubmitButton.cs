using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SubmitButton : Singleton<SubmitButton>
{
    public Button entity { get; private set; }
    public TMPro.TMP_Text text { get; set; }
    
    protected override void Awake()
    {
        base.Awake();
        entity = GetComponent<Button>();
        text = GetComponentInChildren<TMPro.TMP_Text>();
    }

    void Update()
    {
        switch (BoardManager.instance.state)
        {
            case SessionState.prepairing:
                if (text.text != "Create <br> Board")
                {
                    entity.interactable = true;
                    text.text = "CreateBoard";
                }
                break;
            case SessionState.actionSelect:

                if (text.text != "Select Action")
                {
                    entity.interactable = false;
                    text.text = "Select Action";
                }
                break;

            case SessionState.patchInput:
                if (text.text != "SelectPatch")
                {
                    entity.interactable = false;
                    text.text = "SelectPatch";
                }
                break;

            case SessionState.patchMeasure:
                if (text.text != "Measure")
                {
                    entity.interactable = true;
                    text.text = "Measure";
                }
                break;

            case SessionState.patchPlace:
                if (text.text != "Place")
                {
                    entity.interactable = true;
                    text.text = "Place";
                }
                break;
        }
    }
}