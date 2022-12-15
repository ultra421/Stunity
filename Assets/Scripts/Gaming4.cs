using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaming4 : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] Vector2 sideSpeed, jumpSpeed, vel, maxVel;
    bool jumpInput, isGround, isJumping;
    [SerializeField] int maxJumps;
    int jumps;
    [SerializeField] int downForce;
    float timeSinceJumpInput;

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
        //
        isGround = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    private void FixedUpdate()
    {
        vel = rb.velocity;
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
        //Start ray below player
        Vector2 pos = rb.position;
        BoxCollider2D bc2 = GetComponent<BoxCollider2D>();
        float rayDistance = 0.1f;
        Vector2 rayStart = new Vector2(pos.x, pos.y - bc2.bounds.extents.y-0.01f);
      
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down,rayDistance);
        Debug.DrawRay(rayStart, new Vector2(0,-rayDistance),Color.red);

        if (hit.collider == null)
        {
            isGround = false;
            return;
        }

        GameObject collided = hit.transform.gameObject;
        //if collided contains compositie collider then it's the tilemap
        if (collided.GetComponent<CompositeCollider2D>() != null)
        {
            if (!isGround)
            {
                Debug.Log("touched ground: " + collided);
            }
            isGround = true;
            //If going up don't reset jumps
            if (!(vel.y > 0) || IsNearZero(vel.y))
            {
                jumps = 0;
            }
        }

    }

    private void FinalChecks()
    {
        //Assign modified vel to rb
        Debug.Log("Initial vel = " + rb.velocity + " result: " + vel);
        rb.velocity = vel;
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
                Debug.Log("Speed is near 0");
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
}
