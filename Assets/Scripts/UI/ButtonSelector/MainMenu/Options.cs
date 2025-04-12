using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MainMenuButton
{
    private ButtonSelector buttonSelector;
    private OptionMenu optionMenu;
    new void Start()
    {
        base.Start();
        optionMenu = FindObjectOfType<OptionMenu>(true);
        buttonSelector = FindObjectOfType<ButtonSelector>();
    }

    public override void Pressed()
    {
        buttonSelector.canSelect = false;
        optionMenu.gameObject.SetActive(true);
    }
}
