using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isTimePaused = false;

    public void PauseTime()
    {
        Time.timeScale = 0f;
        isTimePaused = true;
    }
    public void MoveTime()
    {
        Time.timeScale = 1f;
        isTimePaused = false;
    }

    //command to erase other gameManager in scene
    public static GameManager inst = null;


    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    #region //Change Scene
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MoveTime();
    }
    #endregion

}
