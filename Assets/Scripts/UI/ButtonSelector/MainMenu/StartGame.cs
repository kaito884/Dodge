using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MainMenuButton
{
    private Fader fader;

    new void Start()
    {
        base.Start();
        fader = FindObjectOfType<Fader>();
    }

    public override void Pressed()
    {
        StartCoroutine(fader.FadeOut());
    }
}
