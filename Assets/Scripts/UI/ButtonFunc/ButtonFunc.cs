using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonFunc : MonoBehaviour
{
    public virtual void Pressed()
    {
        /*Debug.Log("put your function when pressed");*/
    }
    public virtual void SelectedOn()
    {
        /*Debug.Log("put your function when selected");*/
    }
    public virtual void SelectedOff()
    {
        /*Debug.Log("put your function to cancel selected");*/
    }
}
