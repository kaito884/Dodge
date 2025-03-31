using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject clearWindow;
    private Timer timer;
    private Fader fader;
    private bool isClear = false;


    public void ReloadScene(float waitTime = 0)
    {
        StartCoroutine(fader.FadeOut(waitTime, SceneManager.GetActiveScene().buildIndex));
    }
    public void LoadNextScene(float waitTime = 0)
    {
        StartCoroutine(fader.FadeOut(waitTime));
    }



    void Start()
    {
        timer = FindObjectOfType<Timer>();
        fader = FindObjectOfType<Fader>();
    }



    void Update()
    {
        //if timer end, active clear window
        if (timer.timerEnd && !isClear)
        {
            isClear = true;
            clearWindow.SetActive(true);
            GameManager.inst.PauseTime();
        }
    }

}
