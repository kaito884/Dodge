using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInput : MonoBehaviour
{
    //os keys normais atualizam no fixedupdate e gui no update
    private KeyCode[] rightKey = { KeyCode.D, KeyCode.RightArrow };
    private KeyCode[] leftKey = { KeyCode.A, KeyCode.LeftArrow };
    private KeyCode[] upKey = { KeyCode.W, KeyCode.UpArrow, KeyCode.Space, KeyCode.LeftControl, KeyCode.RightControl, KeyCode.LeftShift, KeyCode.RightShift };
    private KeyCode[] downKey = { KeyCode.S, KeyCode.DownArrow };

    private KeyCode[] guiSelectKey = { KeyCode.Return, KeyCode.Space };
    private KeyCode[] guiCancelKey = { KeyCode.Escape, KeyCode.Backspace };
    private KeyCode[] guiRightKey = { KeyCode.D, KeyCode.RightArrow };
    private KeyCode[] guiLeftKey = { KeyCode.A, KeyCode.LeftArrow};
    private KeyCode[] guiUpKey = { KeyCode.W, KeyCode.UpArrow};
    private KeyCode[] guiDownKey = { KeyCode.S, KeyCode.DownArrow};


    //Editable in other scripts
    [HideInInspector] public CommandData down;
    [HideInInspector] public CommandData up;
    [HideInInspector] public CommandData right;
    [HideInInspector] public CommandData left;

    [HideInInspector] public CommandData guiSelect;
    [HideInInspector] public CommandData guiCancel;
    [HideInInspector] public CommandData guiUp;
    [HideInInspector] public CommandData guiLeft;
    [HideInInspector] public CommandData guiDown;
    [HideInInspector] public CommandData guiRight;


    private CommandData[] commandDatas;
    private CommandData[] guiCommandDatas;

    private KeyData[][] keyDatasArray;
    private KeyData[][] guiKeyDatasArray;






    private void Start()
    {
        down = new CommandData();
        up = new CommandData();
        right = new CommandData();
        left = new CommandData();

        guiSelect = new CommandData();
        guiCancel = new CommandData();
        guiUp = new CommandData();
        guiLeft = new CommandData();
        guiDown = new CommandData();
        guiRight = new CommandData();

        commandDatas = new CommandData[] { down, up, right, left};
        commandDatas[0].codes = downKey;
        commandDatas[1].codes = upKey;
        commandDatas[2].codes = rightKey;
        commandDatas[3].codes = leftKey;

        guiCommandDatas = new CommandData[] { guiSelect, guiCancel, guiUp, guiLeft, guiDown, guiRight };
        guiCommandDatas[0].codes = guiSelectKey;
        guiCommandDatas[1].codes = guiCancelKey;
        guiCommandDatas[2].codes = guiUpKey;
        guiCommandDatas[3].codes = guiLeftKey;
        guiCommandDatas[4].codes = guiDownKey;
        guiCommandDatas[5].codes = guiRightKey;



        keyDatasArray = new KeyData[commandDatas.Length][];
        for(int i = 0; i < commandDatas.Length; i++)
        {
            keyDatasArray[i] = new KeyData[commandDatas[i].codes.Length];
            for(int j = 0; j < commandDatas[i].codes.Length; j++)
            {
                keyDatasArray[i][j] = new KeyData();
                keyDatasArray[i][j].code = commandDatas[i].codes[j];
            }
        }
        guiKeyDatasArray = new KeyData[guiCommandDatas.Length][];
        for (int i = 0; i < guiCommandDatas.Length; i++)
        {
            guiKeyDatasArray[i] = new KeyData[guiCommandDatas[i].codes.Length];
            for (int j = 0; j < guiCommandDatas[i].codes.Length; j++)
            {
                guiKeyDatasArray[i][j] = new KeyData();
                guiKeyDatasArray[i][j].code = guiCommandDatas[i].codes[j];
            }
        }
    }

    void FixedUpdate()
    {
        CheckKey(commandDatas, keyDatasArray);
        AtualizeBeforeKey(keyDatasArray);
    }
    void Update()
    {
        CheckKey(guiCommandDatas, guiKeyDatasArray);
        AtualizeBeforeKey(guiKeyDatasArray);
    }



    #region //CheckKeys

    private void CheckKey(CommandData[] commandDatas, KeyData[][] keyDatasArray)
    {
        InicializeKey(commandDatas, keyDatasArray);

        VerifityKey(commandDatas, keyDatasArray);
    }


    //reseta os variaveis do key
    private void InicializeKey(CommandData[] commandDatas, KeyData[][] keyDatasArray)
    {
        for (int i = 0; i < keyDatasArray.Length; i++)
        {
            for(int j = 0; j < keyDatasArray[i].Length; j++)
            {
                keyDatasArray[i][j].down = false;
                keyDatasArray[i][j].on = false;
            }
        }
        for (int i = 0; i < commandDatas.Length; i++)
        {
            commandDatas[i].down = false;
            commandDatas[i].on = false;
        }
    }


    //atualiza os variaveis dos keys
    private void VerifityKey(CommandData[] commandDatas, KeyData[][] keyDatasArray)
    {
        //keyboard key
        for (int i = 0; i < keyDatasArray.Length; i++) {
            for (int j = 0; j < keyDatasArray[i].Length; j++)
            {
                bool keyNow = false;

                if (Input.GetKey(keyDatasArray[i][j].code) || Input.GetKeyDown(keyDatasArray[i][j].code)) 
                    keyNow = true;

                if (keyNow && keyDatasArray[i][j].before)
                {
                    keyDatasArray[i][j].on = true;
                }
                else if (keyNow && !keyDatasArray[i][j].before)
                {
                    keyDatasArray[i][j].down = true;
                    commandDatas[i].down = commandDatas[i].down || keyDatasArray[i][j].down;
                }

                keyDatasArray[i][j].on = keyDatasArray[i][j].on || keyDatasArray[i][j].down;
                commandDatas[i].on = commandDatas[i].on || keyDatasArray[i][j].on;
            }
        }
    }


    //armazena o key no frame anterior
    private void AtualizeBeforeKey(KeyData[][] keyDatasArray)
    {
        foreach (KeyData[] keyDatas in keyDatasArray)
            foreach (KeyData keyData in keyDatas)
                keyData.before = keyData.on;
    }

    #endregion
}
