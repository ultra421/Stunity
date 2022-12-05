using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 ////  06 - 10 - 2022 ////
 Do custom collision using the CheckGround as a base or fix the slowing down when going into a wall
 */

public class gaming2 : MonoBehaviour
{
    public Vector2 pos;
    public Vector2 vel;
    public Vector2 accel,m;
    public float sideAccel;
    public float maxSpeed;
    Rigidbody2D rb;
    private Animator anim;
    public float jumpSpeed;
    public float gravity;
    public bool isAir;
    float jumpExtraTime;
    public float maxJumpExtraTime;
    Vector2 lastPos;
    bool jumpInput;
    float jumpDisableTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pos = transform.position;
        vel = new Vector2(0, 0);
        accel = new Vector2(0, 0);
        anim = GetComponent<Animator>();
        rb.gravityScale = 0;

        sideAccel = 30;
        maxSpeed = 8;
        jumpSpeed = 10;
        gravity = -18;
        isAir = false;
        jumpExtraTime = 0;
        maxJumpExtraTime = 0.1f;
        jumpDisableTime = 0;
        lastPos = pos;
        jumpInput = false;

    }

    // Update is called once per frame
    void Update()
    {

        //TODO: Move this to method or smth
        if (Input.GetKeyDown(KeyCode.Space) && ((!isAir) || (jumpExtraTime <= maxJumpExtraTime && isAir)))
        {
            jumpInput = true;
        }
    }

    private void FixedUpdate()
    {
        //Get info from RB
        pos = transform.position;
        vel = rb.velocity;
        //is airbone?
        if (CheckGround())
        {
            isAir = false;
            jumpExtraTime = 0;
        } else {
            isAir = true;
            jumpExtraTime += Time.deltaTime;
        }
        //Methods
        CheckWalk();
        CheckJump();
        Slowdown();
        CalculateSpeed();
        //Set animation info
        AnimationStuff();
        //FINAL STEP
        lastPos = pos;
        rb.velocity = vel;

    }

    void CheckWalk()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (vel.x > 0)
            {
                accel.x = -sideAccel * 3;
            } else
            {
                accel.x = -sideAccel;
            }

            anim.SetBool("isRun", true);
        }
        if (Input.GetKey(KeyCode.D))
        {

            if (vel.x < 0)
            {
                accel.x = sideAccel * 3;
            } else
            {
                accel.x = sideAccel;
            }

            anim.SetBool("isRun", true);
        }

    }

    void CalculateSpeed ()
    {
        vel.x += accel.x * Time.deltaTime;

        //Vertical accel (gravity)

        if (isAir)
        {
            vel.y += gravity * Time.deltaTime;

            if (Input.GetKey(KeyCode.Space))
            {

            }
        }

        // (X) Limit to maxSpeed maybe change to fast deacceleration?
        if (vel.x > maxSpeed)
        {
            vel.x = maxSpeed;
        } else if (vel.x < -maxSpeed)
        {
            vel.x = -maxSpeed;
        }



        pos.x += vel.x * Time.deltaTime;
    }

    void Slowdown ()
    {
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {

            Debug.Log("Current speed =" + vel.x);

            if (vel.x > 0)
            {
                accel.x = -sideAccel*3;
            }
            else if (vel.x < 0)
            {
                accel.x = sideAccel*3;
            }
            
            //Check if positive accel and vel or negative and if speed near 0 and slow
            if ((accel.x > 0 && vel.x > 0) || (accel.x < 0 && vel.x < 0) || (vel.x > -1 && vel.x < 1 )) {
                Debug.Log("Velocity recieved = " + vel.x + " accel = " + accel.x);
                vel.x = 0;
                accel.x = 0;
                anim.SetBool("isRun",false);
            }

        }

    }

    bool CheckGround ()
    {
        RaycastHit2D hit;
        Collider2D playerCollider = GetComponent<Collider2D>();
        float distance = playerCollider.bounds.extents.y + 0.2f;

        Debug.DrawRay(new Vector3(pos.x, pos.y, 0), new Vector3(0,-distance,0), new Color(100,100,100)); //Origin , end
        hit = Physics2D.Raycast(new Vector3(pos.x, pos.y, 0), Vector3.down, distance); //Origin, direction, distance

        if (hit.collider != null)
        {
            Debug.Log(hit.collider);
            return true;
        } else
        {
            return false;
        }

    }

    void AnimationStuff()
    {
        anim.SetFloat("velAnimCheck", vel.x);

        anim.SetFloat("DeltaX", pos.x - lastPos.x);
        anim.SetFloat("DeltaY", pos.y - lastPos.y);

    }
    
    void CheckJump ()
    {

        if (jumpDisableTime != 0)
        {
            jumpDisableTime -= Time.deltaTime;

            if (jumpDisableTime < 0)
            {
                jumpDisableTime = 0;
            }
        }

        //Check if on ground or extra jump time
        if (jumpInput && jumpExtraTime <= maxJumpExtraTime && jumpDisableTime == 0)
        {
            vel.y = jumpSpeed;
            isAir = true;
            jumpInput = false;
            jumpDisableTime = maxJumpExtraTime + 0.05f;
        }

    }

}
