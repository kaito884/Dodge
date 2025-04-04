using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float startAnimTime;
    [SerializeField] private float activeTime;
    [SerializeField] private float endAnimTime;
    [SerializeField] private float desactiveTime;

    private Animator anime;
    private BoxCollider2D col;

    private readonly string appear = "Appear";
    private readonly string desappear = "Desappear";
    private readonly string timerStr = "timer";

    private float timer = 0;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        anime = GetComponent<Animator>();
        col.enabled = false;
        StartCoroutine(Delay());
    }

    void Update()
    {

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while (true)
        {
            anime.Play(appear);
            timer = 0;
            while(timer <= startAnimTime)
            {
                timer += Time.deltaTime;
                anime.SetFloat(timerStr, timer / startAnimTime);
                yield return null;
            }

            col.enabled = true;
            yield return new WaitForSeconds(activeTime);

            anime.Play(desappear);
            timer = 0;
            while (timer <= endAnimTime)
            {
                timer += Time.deltaTime;
                anime.SetFloat(timerStr, timer / endAnimTime);
                yield return null;
            }
            col.enabled = false;

            yield return new WaitForSeconds(desactiveTime);
        }
    }
}
