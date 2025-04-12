using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPause : ButtonFunc
{
    private ButtonSelector buttonSelector;
    private OptionMenu optionMenu;
    private StageManager stageManager;
    void Start()
    {
        optionMenu = FindObjectOfType<OptionMenu>(true);
        buttonSelector = FindObjectOfType<ButtonSelector>();
        stageManager = FindObjectOfType<StageManager>();
    }

    public override void Pressed()
    {
        buttonSelector.canSelect = false;
        optionMenu.gameObject.SetActive(true);
        stageManager.canSelect = false;
    }
}
