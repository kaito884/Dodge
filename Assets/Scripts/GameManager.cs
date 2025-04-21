using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool isTimePaused = false;

    public int nStages;

    public int[] maxHeartList;
    [HideInInspector] public int maxHeart;
    [HideInInspector] public int heartNum;

    [HideInInspector] public int dificulty = 1;
    [HideInInspector] public const int maxDificulty = 2;



    //PlayerPref Keys
    private string lastStageKey = "lastStageNum";
    [HideInInspector] public string masterVolumeKey = "masterVolume";
    [HideInInspector] public string bgmVolumeKey = "bgmVolume";
    [HideInInspector] public string seVolumeKey = "seVolume";
    private string resolutionXKey = "resolutionX";
    private string resolutionYKey = "resolutionY";
    private string dificultyKey = "dificulty";




    //Time
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

    //Stage
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

    //Volume
    public float GetVolume(string key)
    {
        return PlayerPrefs.GetFloat(key, 0.5f);
    }
    public void SetVolume(string key, float volume)
    {
        volume = Mathf.Clamp(volume, 0, 1);
        PlayerPrefs.SetFloat(key, volume);
        
        if(key == masterVolumeKey)
            SoundManager.Instance.ChangeMasterVolume(volume);
        if (key == bgmVolumeKey)
        {
            SoundManager.Instance.ChangeBgmVolume(volume);
        }
        if (key == seVolumeKey)
            SoundManager.Instance.ChangeSeVolume(volume);
    }

    //Resolution
    public Vector2Int GetResolution()
    {
        Vector2Int resolution = new Vector2Int(PlayerPrefs.GetInt(resolutionXKey, 1920), PlayerPrefs.GetInt(resolutionYKey, 1080));
        return resolution;
    }
    public void SetResolution(Vector2Int resolution)
    {
        PlayerPrefs.SetInt(resolutionXKey, resolution.x);
        PlayerPrefs.SetInt(resolutionYKey, resolution.y);
        Screen.SetResolution(resolution.x, resolution.y, FullScreenMode.Windowed);
    }

    //Dificulty
    public int GetDificulty()
    {
        return PlayerPrefs.GetInt(dificultyKey, 1);
    }
    public void ChangeDificulty(int direction)
    {
        dificulty = Mathf.Clamp(dificulty + direction, 0, maxDificulty);
        PlayerPrefs.SetInt(dificultyKey, dificulty);
        maxHeart = maxHeartList[dificulty];
        heartNum = Mathf.Min(maxHeart, heartNum);
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

        SoundManager.Instance.ChangeMasterVolume(GetVolume(masterVolumeKey));
        SoundManager.Instance.ChangeBgmVolume(GetVolume(bgmVolumeKey));
        SoundManager.Instance.ChangeSeVolume(GetVolume(seVolumeKey));

        dificulty = GetDificulty();
        maxHeart = maxHeartList[dificulty];
        heartNum = maxHeart;
    }


    #region
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MoveTime();
        heartNum = maxHeart;
    }
    #endregion

}
