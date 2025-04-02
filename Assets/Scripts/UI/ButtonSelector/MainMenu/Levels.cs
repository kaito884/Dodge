using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MainMenuButton
{
    private ButtonSelector buttonSelector;
    private LevelSelector levelSelector;
    new void Start()
    {
        base.Start();
        levelSelector = FindObjectOfType<LevelSelector>(true);
        buttonSelector = FindObjectOfType<ButtonSelector>();
    }

    public override void Pressed()
    {
        buttonSelector.canSelect = false;
        levelSelector.gameObject.SetActive(true);
    }
}
