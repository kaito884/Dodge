using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MainMenuButton
{
    new void Start()
    {
        base.Start();
    }

    public override void Pressed()
    {
        Application.Quit();
    }
}
