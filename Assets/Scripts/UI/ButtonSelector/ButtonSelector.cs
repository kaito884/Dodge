using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonSelector : MonoBehaviour
{
    [SerializeField] private ButtonFunc[] buttonFuncs;
    [SerializeField] private GameObject selector = null;
    [SerializeField] private float interval;
    [SerializeField] private int startSelected;

    private CheckInput input;
    private int selected = 0;
    [HideInInspector] public bool canSelect = true;

    void Start()
    {
        input = GetComponent<CheckInput>();
        selected = startSelected;
        buttonFuncs[selected].SelectedOn();
    }
    private void OnEnable()
    {
        canSelect = true;
    }

    private void Update()
    {
        //select button
        if (input.guiSelect.down && canSelect)
        {
            buttonFuncs[selected].Pressed();
            canSelect = false;
        }

        if (!canSelect) return;
        
        //down button
        if(input.guiDown.down && selected < buttonFuncs.Length-1)
        {
            buttonFuncs[selected].SelectedOff();
            selected++;
            buttonFuncs[selected].SelectedOn();
            if(selector)
                selector.transform.position -= new Vector3(0, interval, 0);
        }

        //up button
        if (input.guiUp.down && selected > 0)
        {
            buttonFuncs[selected].SelectedOff();
            selected--;
            buttonFuncs[selected].SelectedOn();
            if (selector)
                selector.transform.position += new Vector3(0, interval, 0);
        }
    }
}
