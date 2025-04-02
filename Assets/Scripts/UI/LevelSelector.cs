using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] int nLevel;
    [SerializeField] Vector2 interval;
    [SerializeField] Vector2 levelPlacement;
    [SerializeField] GameObject canGoLevel;
    [SerializeField] GameObject cannotGoLevel;
    [SerializeField] GameObject background;
    [SerializeField] Color selectedColor;
    [SerializeField] int firstLevelBuildIndex;
    GameObject[,] levels;
    SpriteRenderer[,] sprites;

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

    void Start()
    {
        //iniciacao
        input = GameManager.inst.GetComponent<CheckInput>();
        fader = FindObjectOfType<Fader>();
        buttonSelector = FindObjectOfType<ButtonSelector>();
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
                    level = Instantiate(canGoLevel);
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

        sprites[selected[0], selected[1]].color = selectedColor;
    }


    void Update()
    {
        if (!canSelect) return;

        //desable levelSelector
        if (input.guiCancel.down)
        {
            buttonSelector.canSelect = true;
            gameObject.SetActive(false);
        }

        //select level
        if (input.guiSelect.down)
        {
            canSelect = false;
            StartCoroutine(fader.FadeOut(0,firstLevelBuildIndex+GetLevelIndex(selected)));
        }

        //move selected level
        if (input.guiUp.down && selected[0] != 0)
        {
            sprites[selected[0], selected[1]].color = new Color(1, 1, 1, 1);
            selected[0] -= 1;
            sprites[selected[0], selected[1]].color = selectedColor;
        }
        if (input.guiDown.down && selected[0]+1 != levelPlacement.y && levels[selected[0]+1,selected[1]] != null)
        {
            sprites[selected[0], selected[1]].color = new Color(1, 1, 1, 1);
            selected[0] += 1;
            sprites[selected[0], selected[1]].color = selectedColor;
        }
        if (input.guiLeft.down && selected[1] != 0)
        {
            sprites[selected[0], selected[1]].color = new Color(1, 1, 1, 1);
            selected[1] -= 1;
            sprites[selected[0], selected[1]].color = selectedColor;
        }
        if (input.guiRight.down && selected[1] + 1 != levelPlacement.x && levels[selected[0], selected[1] + 1] != null)
        {
            sprites[selected[0], selected[1]].color = new Color(1, 1, 1, 1);
            selected[1] += 1;
            sprites[selected[0], selected[1]].color = selectedColor;
        }
    }
}
