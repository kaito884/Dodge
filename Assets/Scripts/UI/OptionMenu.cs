using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI masterText;
    [SerializeField] TextMeshProUGUI bgmText;
    [SerializeField] TextMeshProUGUI seText;
    [SerializeField] TextMeshProUGUI resolutionText;
    [SerializeField] Color unselectedColor;
    [SerializeField] Color selectedColor;

    int now = 0;
    const string soundMaster = "SoundMaster";
    const string soundBGM = "SoundBGM";
    const string soundSE = "SoundSE";
    const string resolution = "Resolution";
    private string[] options = { soundMaster, soundBGM, soundSE, resolution}; 

    int nowResolution;
    private Vector2Int[] resolutions16_9 = new Vector2Int[]
    {
        new Vector2Int(1280, 720),   // HD
        new Vector2Int(1600, 900),   // HD+
        new Vector2Int(1920, 1080),  // Full HD
        new Vector2Int(2560, 1440),  // 2K
        new Vector2Int(3840, 2160)   // 4K
    };

    bool waitFrame = true;
    const float volumeMultiplayer = 10f;

    private CheckInput input;
    private ButtonSelector buttonSelector;
    private StageManager stageManager;

    private void Start()
    {
        input = GetComponent<CheckInput>();
        buttonSelector = FindObjectOfType<ButtonSelector>();
        stageManager = FindObjectOfType<StageManager>();

        //get inicial values
        masterText.text = (GameManager.inst.GetVolume(GameManager.inst.masterVolumeKey) * volumeMultiplayer).ToString();
        bgmText.text = (GameManager.inst.GetVolume(GameManager.inst.bgmVolumeKey) * volumeMultiplayer).ToString();
        seText.text = (GameManager.inst.GetVolume(GameManager.inst.seVolumeKey) * volumeMultiplayer).ToString();
        Vector2Int resolution = GameManager.inst.GetResolution();
        for(int i = 0; i < resolutions16_9.Length; i++)
        {
            if (resolutions16_9[i] == resolution) break;
            nowResolution++;
        }
        resolutionText.text = $"{resolution.x}X{resolution.y}";
    }


    void Update()
    {
        //wait one frame
        if (waitFrame)
        {
            waitFrame = false;
            return;
        }

        //desable optionMenu
        if (input.guiCancel.down)
        {
            StartCoroutine(Desable());
        }



        //change selected
        if (input.guiDown.down && now != options.Length - 1)
        {
            now += 1;
            SoundManager.Instance.PlaySE(SESoundData.SE.Hover);
        }
        if (input.guiUp.down && now != 0)
        {
            now -= 1;
            SoundManager.Instance.PlaySE(SESoundData.SE.Hover);
        }



        //sounds

        //reset text color
        bgmText.color = unselectedColor;
        seText.color = unselectedColor;
        masterText.color = unselectedColor;
        //set text
        TextMeshProUGUI text = null;
        if (options[now] == soundMaster)
        {
            text = masterText;
        }
        if (options[now] == soundBGM)
        {
            text = bgmText;
        }
        if (options[now] == soundSE)
        {
            text = seText;
        }
        //change text
        if(text != null)
        {
            if (input.guiLeft.down && text.text != "0")
            {
                text.text = (int.Parse(text.text) - 1).ToString();
                SoundManager.Instance.PlaySE(SESoundData.SE.Select);
            }
            if (input.guiRight.down && text.text != "10")
            {
                text.text = (int.Parse(text.text) + 1).ToString();
                SoundManager.Instance.PlaySE(SESoundData.SE.Select);
            }
            //change volume
            GameManager.inst.SetVolume(GameManager.inst.masterVolumeKey, float.Parse(masterText.text) / volumeMultiplayer);
            GameManager.inst.SetVolume(GameManager.inst.bgmVolumeKey, float.Parse(bgmText.text) / volumeMultiplayer);
            GameManager.inst.SetVolume(GameManager.inst.seVolumeKey, float.Parse(seText.text) / volumeMultiplayer);

            text.color = selectedColor;
        }




        //Resolution
        if (options[now] == resolution)
        {
            if (input.guiLeft.down)
            {
                ChangeResolution(-1);
            }
            if (input.guiRight.down)
            {
                ChangeResolution(1);
            }
            resolutionText.color = selectedColor;
        }
        else resolutionText.color = unselectedColor;
    }

    private void ChangeResolution(int direction)
    {
        nowResolution = Mathf.Clamp(nowResolution + direction, 0, resolutions16_9.Length - 1);
        Vector2Int resolution = resolutions16_9[nowResolution];

        GameManager.inst.SetResolution(resolution);
        resolutionText.text = $"{resolution.x}X{resolution.y}";

        SoundManager.Instance.PlaySE(SESoundData.SE.Select);
    }

    private IEnumerator Desable()
    {
        SoundManager.Instance.PlaySE(SESoundData.SE.Hover);
        yield return null;
        yield return null;
        yield return null;
        print("eter");
        if (stageManager != null)
            stageManager.canSelect = true;
        buttonSelector.canSelect = true;
        waitFrame = true;
        gameObject.SetActive(false);
    }
        
}
