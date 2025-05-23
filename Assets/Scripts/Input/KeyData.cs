using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Storage one key data
/// </summary>

[System.Serializable]
public class KeyData : MonoBehaviour
{
    //variables
    public KeyCode code;
    public bool on;
    public bool down;
    public bool before;

    //inicialize variables
    private void Awake()
    {
        code = KeyCode.A;
        on = false;
        down = false;
        before = false;
    }
}
