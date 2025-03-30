using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    #region //Editable in Inspector
    [Header("Move")]
    [SerializeField] private float xSpeed;
    [SerializeField] private float maxYSpeed;
    [SerializeField] public float maxJumpTime;
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] AnimationCurve fallCurve;
    [Header("Death")]
    [SerializeField] private float gravity; //used only in death animation
    [SerializeField] private float deathInitVel;
    [SerializeField] private float deathXDesaceleration;
    #endregion


    #region //Editable in other scripts
    //player possible states
    public enum State
    {
        Idle,
        Run,
        Jump,
        Fall,
        Hit,
        Die,
        Crouch,
    };
    [HideInInspector] public State state = State.Idle;
    //variables to abbreviate
    [HideInInspector] public State idle = State.Idle;
    [HideInInspector] public State run = State.Run;
    [HideInInspector] public State jump = State.Jump;
    [HideInInspector] public State fall = State.Fall;
    [HideInInspector] public State hit = State.Hit;
    [HideInInspector] public State die = State.Die;
    [HideInInspector] public State crouch = State.Crouch;

    [HideInInspector] public float jumpTime = 0f;
    [HideInInspector] public float fallTime = 0f;
    [HideInInspector] public float xSpeedNow = 0;
    [HideInInspector] public float ySpeedNow = 0;
    [HideInInspector] public bool isRight = true;
    [HideInInspector] public bool isGround = false;
    #endregion



    #region //extern components
    [Header("Player Components")]
    [SerializeField] private TagCheck ground;
    [SerializeField] private TagCheck head;
    [SerializeField] private TagCheck hitbox;
    [SerializeField] private ParticleSystem landParticle;
    [SerializeField] private ParticleSystem[] jumpParticles;
    private Rigidbody2D body;
    private CheckInput input;
    #endregion


    #region //Private
    private bool isHead = false;
    private bool isHit = false;
    private bool canMove = true;
    #endregion





    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        input = GetComponent<CheckInput>();
    }





    void FixedUpdate()
    {
        CheckGroundCollisions();

        if (isHit && state != die) StartDeath();
        if(state != die)
        {
            xSpeedNow = CalcXVelocity();
            ySpeedNow = CalcYVelocity();

            if (canMove) body.velocity = new Vector2(xSpeedNow, ySpeedNow);
        }
        else
        {
            Death();
        }
        
    }


    #region //CheckGroundCollisions

    private void CheckGroundCollisions()
    {
        isGround = ground.IsHit();
        isHead = head.IsHit();
        isHit = hitbox.IsHit();
    }

    #endregion



    #region //Death
    private void StartDeath()
    {
        ChangeState(die);
        canMove = false;
        if(isRight) body.velocity = new Vector2(-deathInitVel, deathInitVel);
        else body.velocity = new Vector2(deathInitVel, deathInitVel);
    }

    private void Death()
    {
        float xSpeed = 0;
        float ySpeed = -gravity;
        if ((body.velocity.x >= 0 && isRight) || (body.velocity.x <= 0 && !isRight)) xSpeed = -body.velocity.x;
        else if (isRight) xSpeed = deathXDesaceleration;
        else xSpeed = -deathXDesaceleration;

        if (isGround) ySpeed = 0;
        body.velocity += new Vector2(xSpeed, ySpeed);
    }

    #endregion



    #region //Velocity

    private float CalcXVelocity()
    {
        float speed = 0;
        if (input.left.on) speed--;
        if (input.right.on) speed++;

        if (isGround)
        {
            if (speed == 0) ChangeState(idle);
            else ChangeState(run);
        }

        if (speed < 0) isRight = false;
        else if(speed > 0) isRight = true;

        return xSpeed * speed;
    }



    private float CalcYVelocity()
    {
        float speed = 0;

        //if is on the ground 
        if (isGround) speed = -maxYSpeed/2;

        //start jump
        if (input.up.down && isGround && state != jump)
        {
            ChangeState(jump);
        }

        //execute jump
        if (state == jump)
        {
            //stop jump
            if (jumpTime >= maxJumpTime || ground.IsEnter() || isHead)
            {
                ChangeState(fall);
            }

            //execute jump
            jumpTime += Time.deltaTime;
            speed = maxYSpeed * jumpCurve.Evaluate(jumpTime / maxJumpTime);
        }

        //start fall
        if (!isGround && state != jump && state != fall)
        {
            ChangeState(fall);
        }

        //execute fall
        if (state == fall)
        {
            //stop fall
            if (isGround)
            {
                ChangeState(idle);
            }

            //execute fall
            else
            {
                fallTime += Time.deltaTime;
                speed = -maxYSpeed * fallCurve.Evaluate(fallTime / maxJumpTime);
            }
        }
        return speed;
    }

    private void ResetYData()
    {
        jumpTime = 0f;
        fallTime = 0f;
    }

    #endregion

    #region //Change state
    void ChangeState(State newState)
    {
        if(state == fall || state == jump)
        {
            ResetYData();
        }
        if(state == fall)
        {
            landParticle.Play();
        }
        if(newState == jump)
        {
            foreach(ParticleSystem jumpParticle in jumpParticles)
                jumpParticle.Play();
        }

        state = newState;
    }
    #endregion
}
