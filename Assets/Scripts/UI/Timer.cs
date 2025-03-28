using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeLimit;
    float timer;
    private TextMeshProUGUI text = null;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        timer = timeLimit;
        if (text == null) print("error");
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        text.text = timer.ToString();
        if (timer <= 0) 
        {
            text.text = "0";
            GameManager.inst.ReloadScene();
        } 
    }
}
