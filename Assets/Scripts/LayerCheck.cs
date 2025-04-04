using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCheck : MonoBehaviour
{
    [SerializeField] private LayerMask checkLayer;
    private bool isHit = false;
    private bool isEnter, isStay, isExit;

    [HideInInspector] public Collider2D col;

    // Chame esta função no FixedUpdate para obter o estado de colisão
    [HideInInspector]
    public bool IsHit()
    {
        if (isEnter || isStay)
        {
            isHit = true;
        }
        if (isExit)
        {
            isHit = false;
        }

        isEnter = false;
        isStay = false;
        isExit = false;
        return isHit;
    }

    [HideInInspector]
    public bool IsEnter()
    {
        return isEnter;
    }

    [HideInInspector]
    public bool IsExit()
    {
        return isExit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & checkLayer) != 0)  // Verifica a layer
        {
            isEnter = true;
            col = collision;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & checkLayer) != 0)  // Verifica a layer
        {
            isStay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & checkLayer) != 0)  // Verifica a layer
        {
            isExit = true;
        }
    }
}
