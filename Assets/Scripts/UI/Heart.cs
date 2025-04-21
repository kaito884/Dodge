using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    [SerializeField] GameObject heartPrefab;
    [SerializeField] float interval = 1.0f;
    GameObject[] hearts;
    int maxHeart = 0;


    void Start()
    {
        maxHeart = GameManager.inst.maxHeart;
        GenerateHeart();
    }


    void Update()
    {
        if(maxHeart != GameManager.inst.maxHeart)
        {
            maxHeart = GameManager.inst.maxHeart;
            GenerateHeart();
        }
        UpdateHeart();
    }



    void GenerateHeart()
    {
        if(hearts != null)
        {
            for(int i = 0; i < hearts.Length; i++)
            {
                Destroy(hearts[i]);
            }
        }

        hearts = new GameObject[GameManager.inst.maxHeart];

        for (int i = 0; i < GameManager.inst.maxHeart; i++)
        {
            Vector2 pos = transform.position + new Vector3(i * interval, 0, 0);
            GameObject heart = Instantiate(heartPrefab, pos, Quaternion.identity);
            heart.transform.SetParent(this.transform);
            heart.GetComponent<SpriteRenderer>().sprite = onSprite;
            hearts[i] = heart;
        }
    }
    void UpdateHeart()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < GameManager.inst.heartNum) hearts[i].GetComponent<SpriteRenderer>().sprite = onSprite;
            else hearts[i].GetComponent<SpriteRenderer>().sprite = offSprite;
        }
    }
}
