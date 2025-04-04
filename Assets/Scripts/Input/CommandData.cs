using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Storage one command data
/// </summary>
/// 
[System.Serializable]
public class CommandData : MonoBehaviour
{
    //variables
    public KeyCode[] codes;
    public bool on;
    public bool down;

    //inicialize variables
    private void Awake()
    {
        codes = new KeyCode[]{};
        on = false;
        down = false;
    }
}
