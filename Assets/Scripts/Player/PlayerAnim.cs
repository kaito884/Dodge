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

    //extern componentes
    [Header("Player Component")]
    [SerializeField] private Animator anime;
    private PlayerMov mov;
    private Rigidbody2D body;

    private Fader fader;
    private Timer[] timer;

    //private components
    private Vector3 playerScale;

    void Start()
    {
        mov = GetComponent<PlayerMov>();
        body = GetComponent<Rigidbody2D>();

        playerScale = this.transform.localScale; //set player default scale

        fader = FindObjectOfType<Fader>();
        timer = FindObjectsOfType<Timer>();
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
    readonly string crouch = "Crouch";

    //animation parameters
    readonly string jumpFallTime = "jumpFallTime";
    readonly string isRun = "isRun";
    readonly string isGround = "isGround";


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

        //death
        if(state == mov.die && clipInfo.clip.name != death)
        {
            anime.Play(death);

            CinemachineImpulseSource impulse = FindObjectOfType<CinemachineImpulseSource>();
            impulse.GenerateImpulse(deathImpulseDirection);

            StartCoroutine(fader.FadeOut(deathAnimTime));

            foreach (Timer t in timer)
                t.stopTimer = true;
        }
    }

    
    void ResetTriggers()
    {
        anime.ResetTrigger(jump);
        anime.ResetTrigger(fall);
        anime.ResetTrigger(endLand);
    }

    #endregion
}