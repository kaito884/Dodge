using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class MainMenuButton : ButtonFunc
{
    [SerializeField] protected TextMeshProUGUI shadow;
    [SerializeField] protected Color pressedColor;

    protected TextMeshProUGUI text;
    protected Color inicialColor;
    public void Start()
    {
        inicialColor = shadow.color;
        text = GetComponent<TextMeshProUGUI>();
    }

    public override void SelectedOn()
    {
        text.enabled = false;
        shadow.color = pressedColor;
    }
    public override void SelectedOff()
    {
        text.enabled = true;
        shadow.color = inicialColor;
    }
}
