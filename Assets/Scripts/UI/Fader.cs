using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] private bool doFadeIn;
    [SerializeField] private bool doFadeOut;
    [SerializeField] private float fadeTime;

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(0, 0, 0, 0);
        if(doFadeIn) StartCoroutine(FadeIn(0));
    }

    public IEnumerator FadeIn(float waitTime)
    {
        if (doFadeIn)
        {
            doFadeIn = false;
            sprite.enabled = true;
            sprite.color = new Color(0, 0, 0, 1);
            yield return null;
            yield return null;
            yield return new WaitForSeconds(waitTime);

            float timer = 0;
            while(timer < fadeTime)
            {
                timer += Time.deltaTime;
                sprite.color = new Color(0, 0, 0, 1 - timer / fadeTime);
                yield return null;
            }

            sprite.color = new Color(0, 0, 0, 0);
        }
    }

    public IEnumerator FadeOut(float waitTime)
    {
        if (doFadeOut)
        {
            doFadeOut = false;
            sprite.color = new Color(0, 0, 0, 0);
            yield return new WaitForSeconds(waitTime);

            float timer = 0;
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                sprite.color = new Color(0, 0, 0, timer / fadeTime);
                yield return null;
            }

            sprite.color = new Color(0, 0, 0, 1);
        }
        GameManager.inst.ReloadScene();
    }
}
