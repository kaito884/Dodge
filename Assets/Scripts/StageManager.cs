using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject clearWindow;
    [SerializeField] private GameObject pauseWindow;
    private CheckInput input;
    private PlayerMov player;
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
    public void LoadScene(int buildIndex, float waitTime = 0)
    {
        StartCoroutine(fader.FadeOut(waitTime, buildIndex));
    }



    void Start()
    {
        timer = FindObjectOfType<Timer>();
        fader = FindObjectOfType<Fader>();
        input = GetComponent<CheckInput>();
        player = FindObjectOfType<PlayerMov>();
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


        //pause window
        if (input.guiCancel.down && player.state != player.die)
        {
            if(!pauseWindow.activeSelf)
            {
                GameManager.inst.PauseTime();
                pauseWindow.SetActive(true);
            }
            else
            {
                GameManager.inst.MoveTime();
                pauseWindow.SetActive(false);
            }
        }
    }

}
