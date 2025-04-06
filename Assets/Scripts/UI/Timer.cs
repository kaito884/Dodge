using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeLimit;
    [HideInInspector] public bool stopTimer = false;
    [HideInInspector] public bool timerEnd = false;
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
        if (stopTimer) return;

        timer -= Time.deltaTime;

        text.text = ((int)timer + 1).ToString();
        if (timer <= 0) 
        {
            text.text = "0";
            timerEnd = true;
        } 
    }
}
