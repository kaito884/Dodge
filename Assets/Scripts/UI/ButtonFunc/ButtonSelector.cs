using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonSelector : MonoBehaviour
{
    [SerializeField] private ButtonFunc[] buttonFuncs;
    [SerializeField] private GameObject selector;
    [SerializeField] private float interval;
    [SerializeField] private int startSelected;

    private CheckInput input;
    private int selected = 0;
    private bool canSelect = true;

    void Start()
    {
        input = GetComponent<CheckInput>();
        selected = startSelected;
        buttonFuncs[selected].SelectedOn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && canSelect)
        {
            buttonFuncs[selected].Pressed();
            canSelect = false;
        }

        if (!canSelect) return;
        
        if(Input.GetKeyDown(KeyCode.S) && selected < buttonFuncs.Length-1)
        {
            buttonFuncs[selected].SelectedOff();
            selected++;
            buttonFuncs[selected].SelectedOn();
            selector.transform.position -= new Vector3(0, interval, 0);
        }

        if (Input.GetKeyDown(KeyCode.W) && selected > 0)
        {
            buttonFuncs[selected].SelectedOff();
            selected--;
            buttonFuncs[selected].SelectedOn();
            selector.transform.position += new Vector3(0, interval, 0);
        }
    }
}
