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
                if (text.text != "Create")
                {
                    entity.interactable = true;
                    text.text = "Create";
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
                if (text.text != "Generate")
                {
                    entity.interactable = true;
                    text.text = "Generate";
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