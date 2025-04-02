using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : ButtonFunc
{
    private StageManager stageManager;
    public void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
    }
    public override void Pressed()
    {
        stageManager.LoadScene(0);
    }
}
