using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : ButtonFunc
{
    [SerializeField] private GameObject pauseWindow;

    public override void Pressed()
    {
        pauseWindow.SetActive(false);
        GameManager.inst.MoveTime();
    }
}
