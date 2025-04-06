using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] Vector2 interval;
    [SerializeField] Vector2 levelPlacement;
    [SerializeField] GameObject[] canGoLevel;
    [SerializeField] int[] levelSpriteStartNum;
    [SerializeField] GameObject cannotGoLevel;
    [SerializeField] GameObject background;
    [SerializeField] Color selectedColor;
    [SerializeField] int firstLevelBuildIndex;
    GameObject[,] levels;
    SpriteRenderer[,] sprites;

    int nLevel;
    private int clearLevel = 4;
    private int[] selected;
    private bool canSelect = true;
    private CheckInput input;
    private ButtonSelector buttonSelector;
    private Fader fader;



    int[] GetLevelPos(int num)
    {
        return new int[] { (int)(num / levelPlacement.x), (int)(num % levelPlacement.x) };
    }
    int GetLevelIndex(int[] num)
    {
        return (int)(num[0] * levelPlacement.x) + (int)(num[1] % levelPlacement.x);
    }
    GameObject GetLevelObject(int num)
    {
        GameObject res = canGoLevel[0];
        for(int i = 0; i < levelSpriteStartNum.Length; i++)
        {
            if (levelSpriteStartNum[i] > num)
                break;
            res = canGoLevel[i];
        }
        return res;
    }

    void Start()
    {
        //iniciacao
        input = GetComponent<CheckInput>();
        fader = FindObjectOfType<Fader>();
        buttonSelector = FindObjectOfType<ButtonSelector>();
        clearLevel = GameManager.inst.GetLastStageNum();
        print(clearLevel);
        nLevel = GameManager.inst.nStages;
        selected = GetLevelPos(clearLevel - 1);

        //////criacao de levels
        int levelCounter = 1;
        levels = new GameObject[(int)levelPlacement.y, (int)levelPlacement.x];
        sprites = new SpriteRenderer[(int)levelPlacement.y, (int)levelPlacement.x];

        Vector2 posCalc = (interval / 2) * (new Vector2(levelPlacement.x - 1, levelPlacement.y - 1));
        Vector3 initPos = new Vector3(-posCalc.x, posCalc.y, 0f);

        for (int i = 0; i < levelPlacement.y; i++)
        {
            for (int j = 0; j < levelPlacement.x; j++)
            {
                levels[i, j] = null;

                if (levelCounter > nLevel) continue;

                //instantiate level
                GameObject level;
                if (levelCounter <= clearLevel)
                {
                    level = Instantiate(GetLevelObject(levelCounter));
                    levels[i, j] = level;
                    sprites[i, j] = level.GetComponent<SpriteRenderer>();
                    level.GetComponentInChildren<TextMeshProUGUI>().text = levelCounter.ToString();
                }
                else
                    level = Instantiate(cannotGoLevel);

                //set position
                level.transform.SetParent(background.transform);
                level.transform.position = background.transform.position;
                level.transform.position += initPos + new Vector3(j * interval.x, i * -interval.y, 0);

                levelCounter++;
            }
        }
        print(selected[0]);
        print(selected[1]);
        sprites[selected[0], selected[1]].color = selectedColor;
    }

    bool waitFrame = true;
    private void OnEnable()
    {
        waitFrame = true;
    }

    void Update()
    {
        if (!canSelect) return;
        if (waitFrame)
        {
            waitFrame = false;
            return;
        }

        //desable levelSelector
        if (input.guiCancel.down)
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.Hover);
            buttonSelector.canSelect = true;
            gameObject.SetActive(false);
        }

        //select level
        if (input.guiSelect.down)
        {
            canSelect = false;
            StartCoroutine(fader.FadeOut(0,firstLevelBuildIndex+GetLevelIndex(selected)));
            SoundManager.Instance.PlaySE(SESoundData.SE.Select);
        }

        //move selected level
        if (input.guiUp.down && selected[0] != 0)
        {
            sprites[selected[0], selected[1]].color = new Color(1, 1, 1, 1);
            selected[0] -= 1;
            sprites[selected[0], selected[1]].color = selectedColor;
            SoundManager.Instance.PlaySE(SESoundData.SE.Hover);
        }
        if (input.guiDown.down && selected[0]+1 != levelPlacement.y && levels[selected[0]+1,selected[1]] != null)
        {
            sprites[selected[0], selected[1]].color = new Color(1, 1, 1, 1);
            selected[0] += 1;
            sprites[selected[0], selected[1]].color = selectedColor;
            SoundManager.Instance.PlaySE(SESoundData.SE.Hover);
        }
        if (input.guiLeft.down && selected[1] != 0)
        {
            sprites[selected[0], selected[1]].color = new Color(1, 1, 1, 1);
            selected[1] -= 1;
            sprites[selected[0], selected[1]].color = selectedColor;
            SoundManager.Instance.PlaySE(SESoundData.SE.Hover);
        }
        if (input.guiRight.down && selected[1] + 1 != levelPlacement.x && levels[selected[0], selected[1] + 1] != null)
        {
            sprites[selected[0], selected[1]].color = new Color(1, 1, 1, 1);
            selected[1] += 1;
            sprites[selected[0], selected[1]].color = selectedColor;
            SoundManager.Instance.PlaySE(SESoundData.SE.Hover);
        }
    }
}
