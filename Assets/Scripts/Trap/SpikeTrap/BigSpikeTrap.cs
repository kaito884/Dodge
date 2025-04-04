using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSpikeTrap : MonoBehaviour
{
    [SerializeField] private float checkDistance;
    [SerializeField] private LayerMask groundMask;

    [Header("Time")]
    [SerializeField] private float delay;
    [SerializeField] private float noticeTime;
    [SerializeField] private float activeAnimTime;
    [SerializeField] private float activeTime;
    [SerializeField] private float desactiveAnimTime;
    [SerializeField] private float desactiveTime;


    private Transform player;
    private Animator anime;
    private PolygonCollider2D col;
    private ParticleSystem particle;

    private float timer = 0;
    private readonly string appear = "Active";
    private readonly string desappear = "Desactive";
    private readonly string notice = "Notice";
    private readonly string timerStr = "timer";

    void Start()
    {
        player = FindObjectOfType<PlayerMov>().transform;
        col = GetComponent<PolygonCollider2D>();
        anime = GetComponent<Animator>();
        particle = GetComponent<ParticleSystem>();

        var main = particle.main;
        main.duration = noticeTime;

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(DetectGround());
    }

    IEnumerator DetectGround()
    {
        while (true)
        {
            RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.down, checkDistance, groundMask);
            if (hit.collider == null)
            {
                yield return new WaitForSeconds(activeTime);
                yield return new WaitForSeconds(desactiveTime);
            }
            else
            {
                transform.position = hit.point;

                anime.Play(notice);
                particle.Play();
                SoundManager.Instance.PlaySE(SESoundData.SE.BigSpikeNotice);
                timer = 0;
                while (timer <= noticeTime)
                {
                    timer += Time.deltaTime;
                    anime.SetFloat(timerStr, timer / noticeTime);
                    yield return null;
                }

                col.enabled = true;

                anime.Play(appear);
                SoundManager.Instance.PlaySE(SESoundData.SE.BigSpikeActive);
                timer = 0;
                while (timer <= activeAnimTime)
                {
                    timer += Time.deltaTime;
                    anime.SetFloat(timerStr, timer / activeAnimTime);
                    yield return null;
                }

                yield return new WaitForSeconds(activeTime);

                anime.Play(desappear);
                timer = 0;
                while (timer <= desactiveAnimTime)
                {
                    timer += Time.deltaTime;
                    anime.SetFloat(timerStr, timer / desactiveAnimTime);
                    yield return null;
                }

                col.enabled = false;

                yield return new WaitForSeconds(desactiveTime);
            }
        }
    }
}