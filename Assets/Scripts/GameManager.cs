using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isTimePaused = false;
    public int nStages;
    private string lastStageKey = "lastStageNum";

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
    public void SetLastStageNum(int num)
    {
        num = Mathf.Max(PlayerPrefs.GetInt(lastStageKey, 1),num, 1);
        num = Mathf.Min(nStages, num);
        PlayerPrefs.SetInt(lastStageKey, num);
    }
    public int GetLastStageNum()
    {
        return PlayerPrefs.GetInt(lastStageKey, 1);
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


    #region
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MoveTime();
    }
    #endregion

}
