using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInput : MonoBehaviour
{
    //os keys normais atualizam no fixedupdate e gui no update
    private KeyCode[] rightKey = { KeyCode.D, KeyCode.RightArrow };
    private KeyCode[] leftKey = { KeyCode.A, KeyCode.LeftArrow };
    private KeyCode[] upKey = { KeyCode.W, KeyCode.UpArrow, KeyCode.Space };
    private KeyCode[] downKey = { KeyCode.S, KeyCode.DownArrow };

    private KeyCode[] guiSelectKey = { KeyCode.Return, KeyCode.Space };
    private KeyCode[] guiCancelKey = { KeyCode.Escape, KeyCode.Backspace };
    private KeyCode[] guiRightKey = { KeyCode.D, KeyCode.RightArrow };
    private KeyCode[] guiLeftKey = { KeyCode.A, KeyCode.LeftArrow};
    private KeyCode[] guiUpKey = { KeyCode.W, KeyCode.UpArrow};
    private KeyCode[] guiDownKey = { KeyCode.S, KeyCode.DownArrow};


    //Editable in other scripts
    [HideInInspector] public KeyData down;
    [HideInInspector] public KeyData up;
    [HideInInspector] public KeyData right;
    [HideInInspector] public KeyData left;

    [HideInInspector] public KeyData guiSelect;
    [HideInInspector] public KeyData guiCancel;
    [HideInInspector] public KeyData guiUp;
    [HideInInspector] public KeyData guiLeft;
    [HideInInspector] public KeyData guiDown;
    [HideInInspector] public KeyData guiRight;

    //local variable
    private KeyData[] keys = {};
    private KeyData[] guiKeys = { };






    private void Start()
    {
        down = new KeyData();
        up = new KeyData();
        right = new KeyData();
        left = new KeyData();

        guiSelect = new KeyData();
        guiCancel = new KeyData();
        guiUp = new KeyData();
        guiLeft = new KeyData();
        guiDown = new KeyData();
        guiRight = new KeyData();

        keys = new KeyData[] { down, up, right, left };
        guiKeys = new KeyData[] { guiSelect, guiCancel, guiUp, guiLeft, guiDown, guiRight };

        SetKeyNames();
    }



    void FixedUpdate()
    {
        CheckKey(keys);
        AtualizeBeforeKey(keys);
    }
    void Update()
    {
        CheckKey(guiKeys);
        AtualizeBeforeKey(guiKeys);
    }





    //atualiza o nome dos keys
    public void SetKeyNames()
    {
        down.codes = downKey;
        up.codes = upKey;
        right.codes = rightKey;
        left.codes = leftKey;

        guiSelect.codes = guiSelectKey;
        guiCancel.codes = guiCancelKey;
        guiRight.codes = guiRightKey;
        guiLeft.codes = guiLeftKey;
        guiUp.codes = guiUpKey;
        guiDown.codes = guiDownKey;
    }


    #region //CheckKeys

    private void CheckKey(KeyData[] keys)
    {
        InicializeKey(keys);

        VerifityKey(keys);
    }


    //reseta os variaveis do key
    private void InicializeKey(KeyData[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].down = false;
            keys[i].on = false;
        }
    }


    //atualiza os variaveis dos keys
    private void VerifityKey(KeyData[] keys)
    {
        //keyboard key
        foreach (KeyData key in keys)
        {
            bool keyNow = false;

            foreach (KeyCode keyCode in key.codes)
            {
                if (Input.GetKey(keyCode) || Input.GetKeyDown(keyCode))
                {
                    keyNow = true;
                    break;
                }
            }
            if (keyNow && key.before)
            {
                key.on = true;
            }
            else if (keyNow && !key.before)
            {
                key.down = true;
            }
            key.on = key.on || key.down;
        }
    }


    //armazena o key no frame anterior
    private void AtualizeBeforeKey(KeyData[] keys)
    {
        foreach (KeyData key in keys)
            key.before = key.on;
    }

    #endregion
}
