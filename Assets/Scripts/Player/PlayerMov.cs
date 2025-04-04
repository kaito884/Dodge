using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    #region //Editable in Inspector
    [Header("Move")]
    [SerializeField] private float xSpeed;
    [SerializeField] private float runSoundInterval;
    [SerializeField] private float maxYSpeed;
    [SerializeField] private float maxHoverTime;
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
        Squat,
        Die,
        Hover,
    };
    [HideInInspector] public State state = State.Idle;
    //variables to abbreviate
    [HideInInspector] public State idle = State.Idle;
    [HideInInspector] public State run = State.Run;
    [HideInInspector] public State jump = State.Jump;
    [HideInInspector] public State fall = State.Fall;
    [HideInInspector] public State hit = State.Hit;
    [HideInInspector] public State squat = State.Squat;
    [HideInInspector] public State die = State.Die;
    [HideInInspector] public State hover = State.Hover;

    [HideInInspector] public float jumpTime = 0f;
    [HideInInspector] public float fallTime = 0f;
    [HideInInspector] public float xSpeedNow = 0;
    [HideInInspector] public float ySpeedNow = 0;
    [HideInInspector] public bool isRight = true;
    [HideInInspector] public bool isGround = false;
    #endregion



    #region //extern components
    [Header("Player Components")]
    [SerializeField] private LayerCheck ground;
    [SerializeField] private LayerCheck head;
    [SerializeField] private LayerCheck hitbox;
    [SerializeField] private LayerCheck squatHitbox;
    [SerializeField] private ParticleSystem landParticle;
    [SerializeField] private ParticleSystem[] jumpParticles;
    [SerializeField] private BoxCollider2D normalCollider;
    [SerializeField] private BoxCollider2D squatCollider;
    private Rigidbody2D body;
    private CheckInput input;
    #endregion


    #region //Private
    private bool isHead = false;
    private bool isHit = false;
    private bool canMove = true;
    private bool canHover = true;
    private float hoverTime = 0f;
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
        if (state != die)
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
        isHit = hitbox.IsHit() || squatHitbox.IsHit();
    }

    #endregion



    #region //Death
    private void StartDeath()
    {
        ChangeState(die);
        canMove = false;
        if (isRight) body.velocity = new Vector2(-deathInitVel, deathInitVel);
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

            //start squat
            if (input.down.on)
            {
                ChangeState(squat);
                normalCollider.enabled = false;
                squatCollider.enabled = true;
            }
            //squat
            if (state == squat)
            {
                speed = 0;
                //stop squat
                if (!input.down.on)
                {
                    ChangeState(idle);
                }
            }
        }

        if (speed < 0) isRight = false;
        else if (speed > 0) isRight = true;

        return xSpeed * speed;
    }



    private float CalcYVelocity()
    {
        float speed = 0;

        //if is on the ground 
        if (isGround) speed = -maxYSpeed / 2;

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
            jumpTime += Time.fixedDeltaTime;
            speed = maxYSpeed * jumpCurve.Evaluate(jumpTime / maxJumpTime);
        }

        //start hover
        if (input.up.down && !isGround && state != hover && canHover)
        {
            ChangeState(hover);
            canHover = false;
        }

        //execute hover
        if (state == hover)
        {
            speed = 0f;
            hoverTime += Time.fixedDeltaTime;
            if (!input.up.on || hoverTime >= maxHoverTime)
            {
                ChangeState(fall);
            }
        }

        if (isGround) canHover = true;

        //start fall
        if (!isGround && state != jump && state != fall && state != hover)
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
                fallTime += Time.fixedDeltaTime;
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
        if (state == fall || state == jump)
        {
            ResetYData();
        }
        if (state == fall && isGround)
        {
            landParticle.Play();
            SoundManager.Instance.PlaySE(SESoundData.SE.Land);
        }
        if (newState == jump)
        {
            foreach (ParticleSystem jumpParticle in jumpParticles)
                jumpParticle.Play();
            SoundManager.Instance.PlaySE(SESoundData.SE.Jump);
        }
        if (newState == run && state != newState)
        {
            StartCoroutine(RunSound());
        }
        if (newState == die)
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.Damage);
        }
        if (state == hover)
        {
            hoverTime = 0f;
        }
        if(state == squat)
        {
            normalCollider.enabled = true;
            squatCollider.enabled = false;
        }

        state = newState;
    }

    private bool isRunSound = false;
    private IEnumerator RunSound()
    {
        if (isRunSound) yield break;
        isRunSound = true;
        do
        {
            yield return new WaitForSeconds(runSoundInterval/2);
            SoundManager.Instance.PlaySE(SESoundData.SE.StepRock);
            yield return new WaitForSeconds(runSoundInterval/2);
        }
        while (state == run);
        isRunSound = false;
    }
    #endregion
}
