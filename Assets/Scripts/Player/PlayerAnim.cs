using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//manage player animation
public class PlayerAnim : MonoBehaviour
{
    //editable in inspector
    [SerializeField] private Vector2 deathImpulseDirection;
    [SerializeField] private float deathAnimTime;
    [SerializeField] private float blinkInterval;
    [SerializeField] private Vector2 hitImpulseDirection;

    //extern componentes
    [Header("Player Component")]
    [SerializeField] private Animator anime;
    [SerializeField] SpriteRenderer sprite;
    private PlayerMov mov;
    private Rigidbody2D body;
    CinemachineImpulseSource impulse;

    private StageManager stageManager;
    private Timer[] timer;

    //private components
    private Vector3 playerScale;
    private bool isBlinking;



    void Start()
    {
        mov = GetComponent<PlayerMov>();
        body = GetComponent<Rigidbody2D>();

        playerScale = this.transform.localScale; //set player default scale

        stageManager = FindObjectOfType<StageManager>();
        timer = FindObjectsOfType<Timer>();
        impulse = FindObjectOfType<CinemachineImpulseSource>();
    }



    void Update()
    {
        Animate();
    }




    #region Animate

    //animation variables
    PlayerMov.State state;
    AnimatorClipInfo clipInfo;
    AnimatorStateInfo stateInfo;

    //animation names
    readonly string idle = "Idle";
    readonly string run = "Run";
    readonly string jump = "Jump";
    readonly string fall = "Fall";
    readonly string endLand = "EndLand";
    readonly string land = "Land";
    readonly string death = "Death";
    readonly string hit = "Hit";

    //animation parameters
    readonly string jumpFallTime = "jumpFallTime";
    readonly string isRun = "isRun";
    readonly string isGround = "isGround";
    readonly string isHover = "isHover";
    readonly string isSquat = "isSquat";


    void Animate()
    {
        //abbreviate
        state = mov.state;
        clipInfo = anime.GetCurrentAnimatorClipInfo(0)[0];
        stateInfo = anime.GetCurrentAnimatorStateInfo(0);

        //set parameters
        anime.SetBool(isRun, mov.xSpeedNow != 0);
        anime.SetBool(isGround, mov.isGround);

        //turn
        if (mov.isRight)
            body.transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);
        else
            body.transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z);



        //link player state with animation

        //reset all triggers
        ResetTriggers();

        //jump
        if (state == mov.jump)
        {
            anime.SetFloat(jumpFallTime, (mov.jumpTime / mov.maxJumpTime));
            anime.SetTrigger(jump);
        }

        //fall
        if (state == mov.fall)
        {
            anime.SetFloat(jumpFallTime, (mov.fallTime / mov.maxJumpTime));
            anime.SetTrigger(fall);
        }

        //land
        if (clipInfo.clip.name == land && stateInfo.normalizedTime > 0.9)
        {
            anime.SetTrigger(endLand);
        }

        //crouch
        if (state == mov.squat)
            anime.SetBool(isSquat, true);
        if (state != mov.squat)
            anime.SetBool(isSquat, false);

        //death
        if (state == mov.die && clipInfo.clip.name != death)
        {
            anime.Play(death);

            impulse.GenerateImpulse(deathImpulseDirection);

            stageManager.ReloadScene(deathAnimTime);

            foreach (Timer t in timer)
                t.stopTimer = true;
        }

        //hit
        if(mov.isDamage && !isBlinking)
        {
            impulse.GenerateImpulse(hitImpulseDirection);
            StartCoroutine(Blink());
        }

        //hover
        if(state == mov.hover)
            anime.SetBool(isHover, true);
        if (state != mov.hover)
            anime.SetBool(isHover, false);
    }

    private IEnumerator Blink()
    {
        isBlinking = true;
        float timer = 0f;

        while (mov.isDamage)
        {
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        sprite.enabled = true;
        isBlinking = false;
    }


void ResetTriggers()
    {
        anime.ResetTrigger(jump);
        anime.ResetTrigger(fall);
        anime.ResetTrigger(endLand);
    }

    #endregion
}