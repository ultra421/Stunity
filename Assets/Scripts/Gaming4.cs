using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaming4 : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] Vector2 sideSpeed, jumpSpeed, vel, maxVel;
    bool jumpInput, isGround, isJumping, isTouchingWall;
    [SerializeField] int maxJumps;
    int jumps;
    [SerializeField] int downForce;
    float timeSinceJumpInput, airTime, maxAirtime;
    [SerializeField] Animator animator;
    bool stompedEnemy;
    [SerializeField]AudioSource stomp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vel = rb.velocity;
        //Defaults
        maxVel = new Vector2(5, 100);
        sideSpeed = new Vector2(20,0);
        jumpSpeed = new Vector2(0,350);
        jumpInput = false;
        maxJumps = 2;
        downForce = 25;
        timeSinceJumpInput = 0;
        maxAirtime = 0.10f;
        //
        isGround = false;
        isTouchingWall = false;
        airTime = 0;
        stompedEnemy = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    private void FixedUpdate()
    {
        SetStartFrameVars(); //Always first method
        CheckGround();
        Movement();
        //FINAL
        FinalChecks();
    }

    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
            timeSinceJumpInput = 0;
        }
    }

    private void Movement()
    {
        InitialChecks();
        CheckMaxSpeed();
        StopHorizontalSpeed();
        MultiplyVerticalHoldingJump();
    }

    private void InitialChecks()
    {

        //If want to go left and velocity is for right side
        if (Input.GetKey(KeyCode.A) && vel.x > 0)
        {
            rb.AddForce(-sideSpeed * 2);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-sideSpeed);
        }

        //If want to go right and velocity is for left side
        if (Input.GetKey(KeyCode.D) && vel.x < 0)
        {
            rb.AddForce(sideSpeed * 2);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(sideSpeed);
        }

        //If jumpInput and is ground or in air and not max jumps
        if (jumpInput && (isGround || (!isGround && jumps < maxJumps)))
        {
            //Reset vertical velocity
            vel.y = 0;
            rb.AddForce(jumpSpeed);
            jumpInput = false;
            jumps++;
            Debug.Log("jumpin / jumps = " + jumps);
        } else if (isTouchingWall && jumpInput && !isGround){ 
            //Walljump
            vel.y = 0;
            rb.AddForce(jumpSpeed);
            jumpInput = false;
        } else if (jumpInput && !isGround && airTime < maxAirtime)
        {
            //Extra time for jump
            vel.y = 0;
            rb.AddForce(jumpSpeed);
            jumpInput = false;
        }
    }
    private void CheckMaxSpeed()
    {
        //Right
        if (vel.x > maxVel.x)
        {
            vel.x = maxVel.x;
        }
        //Left
        if (vel.x < -maxVel.x)
        {
            vel.x = -maxVel.x;
        }
    }

    private void CheckGround()
    {
        //Start ray below player, to their left and right
        Vector2 pos = rb.position;
        BoxCollider2D bc2 = GetComponent<BoxCollider2D>();
        Vector2 rayStart1 = new Vector2(bc2.bounds.center.x-bc2.bounds.extents.x, bc2.bounds.center.y - bc2.bounds.extents.y); //left
        Vector2 rayStart2 = new Vector2(bc2.bounds.center.x+bc2.bounds.extents.x, bc2.bounds.center.y - bc2.bounds.extents.y); //right
        RaycastHit2D hit1 = Physics2D.Raycast(rayStart1, Vector2.down, 0.15f); //left
        RaycastHit2D hit2 = Physics2D.Raycast(rayStart2, Vector2.down, 0.15f); //Right
        Debug.DrawRay(rayStart1, new Vector2(0, -0.15f), Color.red);
        Debug.DrawRay(rayStart2, new Vector2(0, -0.15f), Color.red);

        if (hit1.collider == null && hit2.collider == null)
        {
            isGround = false;
            return;
        }

        GameObject collided = null;
        if (hit1.collider != null)
        {
            collided = hit1.transform.gameObject;
        } else if (hit2.collider != null) {
            collided = hit2.transform.gameObject;
        }

        //if collided contains compositie collider then it's the tilemap
        if (collided.GetComponent<CompositeCollider2D>() != null)
        {
            if (!isGround)
            {
                Debug.Log("touched ground: " + collided);
            }
            isGround = true;
            airTime = 0;
            //If going up don't reset jumps
            if (!(vel.y > 0) || IsNearZero(vel.y))
            {
                jumps = 0;
            }
        }

    }

    private void FinalChecks()
    {
        if (stompedEnemy)
        {
            vel.y = 0;
            rb.AddForce(jumpSpeed);
            stompedEnemy = false;
        }
        rb.velocity = vel;
        SetAnimatiorVars();
    }
    
    //Additional force applied depends on if holding jump or not
    private void MultiplyVerticalHoldingJump()
    {
        if (!isGround)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(new Vector2(0, -downForce * 0.25f));
            } else
            {
                rb.AddForce(new Vector2(0,-downForce));
            }
        }
    }

    private bool IsNearZero(float input)
    {
        if ((input < 0.25f && input > -0.25f) && input != 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void StopHorizontalSpeed()
    {

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {

            if (IsNearZero(vel.x))
            {
                //Debug.Log("Speed is near 0");
                vel.x = 0;
                return;
            }

            //First check if on ground or air, change multilpier depending on that
            float slowDownSpeed;
            if (isGround) // Ground
            {
                slowDownSpeed = sideSpeed.x * 0.8f;
            } else // Air
            {
                slowDownSpeed = sideSpeed.x * 0.4f;
            }

            if (vel.x > 0) //Going right
            {
                rb.AddForce(new Vector2(-slowDownSpeed,0));
            }
            else if (vel.x < 0) //Going left
            {
                rb.AddForce(new Vector2(slowDownSpeed,0));
            }
        }

    }

    private void SetStartFrameVars()
    {
        vel = rb.velocity; //Local vel var to modify the rigidbody's velocity

        //Time since jump has been pressed, disable jumpInput after 0.15s have passed
        if (jumpInput && timeSinceJumpInput > 0.15f)
        {
            jumpInput = false;
            timeSinceJumpInput = 0;
        } else if (jumpInput)
        {
            timeSinceJumpInput += Time.deltaTime;
        }

        //Get if touching wall
        WallCheckScript wcs = rb.transform.Find("WallCheck").GetComponent<WallCheckScript>();
        isTouchingWall = wcs.IsTouchingWall;

        //Sum time to airtime counter
        if (!isGround)
        {
            airTime += Time.deltaTime;
        }

        //Substract 1 jump if on air and surpassed maxAirTime
        if (!isGround && airTime>maxAirtime && jumps == 0)
        {
            jumps++;
        }
    }

    private void SetAnimatiorVars()
    {
        animator.SetFloat("SpeedX", vel.x);
        animator.SetFloat("SpeedY", vel.y);
        animator.SetBool("isGround", isGround);
        animator.SetInteger("jumps", jumps);
        //Flip sprite
        if (vel.x < 0)
        {
            gameObject.transform.localScale = new Vector3(-3, 3, 1); //flip to the left
        } else if (vel.x > 0)
        {
            gameObject.transform.localScale = new Vector3(3, 3, 1); //flip to the right
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        DrawContacts(collision);
    }

    private void DrawContacts(Collision2D collision)
    {
        foreach (ContactPoint2D hit in collision.contacts)
        {
            Debug.DrawLine(this.rb.transform.position, hit.point, Color.red);
        }
    }

    public void StompedEnemy ()
    {
        stomp.Play();
        stompedEnemy = true;
    }

}
