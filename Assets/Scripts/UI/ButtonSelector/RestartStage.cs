using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartStage : ButtonFunc
{
    private StageManager stageManager;
    public void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
    }
    public override void Pressed()
    {
        stageManager.ReloadScene();
    }
}
