using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gaming3 : MonoBehaviour
{

    /* 
        Do checks on all left right and top -> should bottom be last?
     */

    [SerializeField] private Vector2 pos, speed, accel, defaultAccel, deAccel;
    private Vector2 lastPos;
    [SerializeField] private Vector2 maxSpeed, deAccelMultiplier, speedMultiplier;
    [SerializeField] int jumps, maxJumps;
    [SerializeField] private float maxExtraJumpTime,gravity,jumpSpeed,extraJumpTime,airAccelerationMultiplier;
    private bool jumpInput;
    [SerializeField] private bool isJumping,isGround, isTouchWall;
    private Collider2D coll2D;

    // Start is called before the first frame update
    void Start()
    {   
        //Position stuff
        pos = transform.position;
        speed = new Vector2(0, 0);
        accel = new Vector2(0, 0);
        lastPos = pos;
        //Defaults
        gravity = -25;
        defaultAccel = new Vector2(40, 30);
        deAccelMultiplier = new Vector2(2.5f, 0);
        deAccel = new Vector2(defaultAccel.x*deAccelMultiplier.x, 10);
        maxSpeed = new Vector2(7, 10);
        speedMultiplier = new Vector2(1, 1);
        //Jump stuff
        jumpSpeed = 7;
        extraJumpTime = 0;
        maxExtraJumpTime = 0.15f;
        jumpInput = false;
        jumps = 0;
        maxJumps = 2;
        isJumping = false;
        isGround = false;
        airAccelerationMultiplier = 0.7f;
        //Collision
        coll2D = GetComponent<Collider2D>();
        isTouchWall = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForJumpInput();
    }

    private void FixedUpdate()
    {
        getValues();
        GravityCaculations();
        InitialSpeedCalculations();
        CheckForSideInput();
        FrictionCalculations();
        CheckMultiplier();
        //Final step of speed calculation (calculate the pos)
        InitialPosCalculations();
        //Collisions after this comment!
        CollisionChecks();
        //Below will be final
        AssignPosToTransform();
    }

    void CheckForJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }
    }

    void CheckForSideInput()
    {
        //TODO: make the manual multiplier not shit
        if (Input.GetKey(KeyCode.A) && speed.x > 0) //Accelerate left faster if opposite side
        {
            accel.x = -defaultAccel.x * deAccelMultiplier.x * 0.75f;
        } else if (Input.GetKey(KeyCode.A))
        {
            accel.x = -defaultAccel.x;
        }

        if (Input.GetKey(KeyCode.D) && speed.x < 0) //Accelerate right faster if opposite side
        {
            accel.x = defaultAccel.x * deAccelMultiplier.x * 0.75f;
        } else if (Input.GetKey(KeyCode.D))
        {
            accel.x = defaultAccel.x;
        }

        //Check if airbone then reduce acceleration by multiplier
        if (!isGround)
        {
            accel.x *= airAccelerationMultiplier;
        }

    }

    /// <summary>
    /// <c>SpeedAndPosCalculations</c> will calculate position without collisions
    /// </summary>
    void InitialSpeedCalculations()
    {
        //Horizontal speed calculations
        speed.x += accel.x * Time.deltaTime;

        //Vertical calculations
        //Add 1 jump (removing 1 from max) when in air for more than the extra time
        if (jumps == 0 && extraJumpTime > maxExtraJumpTime)
        {
            jumps++;
        }
        //Jump
        if (jumpInput && jumps < maxJumps && (isGround || extraJumpTime < maxExtraJumpTime))
        {
            jumpInput = false;
            isJumping = true;
            speed.y = jumpSpeed;
            jumps++;
            Debug.Log("isGround?" + isGround + " jumps?" + jumps + "extraJumpTime?" + extraJumpTime);
            
        } else
        {
            jumpInput = false;
        }

    }
    /// <summary>
    /// Final position function, no more after this!
    /// </summary>
    void AssignPosToTransform()
    {
        transform.position = pos;
    }

    void FrictionCalculations ()
    {
        //If within range of maxSpeed to maxSpeed+- 0.3f assign speed to maxSpeed
        if (speed.x < -maxSpeed.x && speed.x >= -maxSpeed.x-0.95f) //Going left
        {
            speed.x = -maxSpeed.x;
        } else if (speed.x > maxSpeed.x && speed.x <= maxSpeed.x+0.95f) //Going right
        {
            speed.x = maxSpeed.x;
        }
        //Gradual slowdown if higher than 1 of maxSpeed
        else if (speed.x > maxSpeed.x+1) // Going right
        {
            speed.x -= deAccel.x * Time.deltaTime;
        }
        else if (speed.x < -maxSpeed.x-1) //Going left
        {
            speed.x += deAccel.x * Time.deltaTime;
        }
        //If no input
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            //Slowdown if no input
            if (speed.x > 0) //Going right
            {
                speed.x -= deAccel.x * Time.deltaTime;
            } else if (speed.x < 0) //Going left
            {
                speed.x += deAccel.x * Time.deltaTime;
            }
            //Slowdown to 0 if near
            if (speed.x > -0.5f && speed.x < 0.5f)
            {
                speed.x = 0;
                accel.x = 0;
            }

        }
        //Vertical calculations
        if (speed.y > maxSpeed.y && speed.y <= maxSpeed.y + 0.95f) //going up
        {
            speed.y = maxSpeed.y;
        }
        else if (speed.y < -maxSpeed.y && speed.y >= -maxSpeed.y - 0.95f) //going down
        {
            speed.y = -maxSpeed.y;
        }
        //Gradual slowdown if higher than 1 of maxSpeed
        else if (speed.y > maxSpeed.y + 1)
        {
            speed.y -= deAccel.y * Time.deltaTime;
        }
        else if (speed.y < -maxSpeed.y - 1)
        {
            speed.y += deAccel.y * Time.deltaTime;
        }
    }

    private void InitialPosCalculations()
    {
        pos.x += speed.x * Time.deltaTime * speedMultiplier.x;
        pos.y += speed.y * Time.deltaTime * speedMultiplier.y;
    }

    private void getValues ()
    {
        pos = transform.position;

        //Conditionals below
        if (!isGround && extraJumpTime < maxExtraJumpTime)
        {
            extraJumpTime += Time.deltaTime;
        } else if (isGround);
        {
            isJumping = false;
            extraJumpTime = 0;
        }

    }

    private void GravityCaculations()
    {
        if (!isGround)
        {
            speed.y += gravity * Time.deltaTime;
        } if (isGround)
        {
            speed.y = 0;
            jumps = 0;
        }
    }
    private void CheckMultiplier()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //TODO: Multiplier should smoothly go back to 1 if not pressing anything
            //going up
            if (speed.y > 0)
            {
                speedMultiplier.y = 2f;
            }

            //falling down
            else if (speed.y < 0)
            {
                speedMultiplier.y = 0.8f;
            }
            else
            {
                speedMultiplier.y = 1;
            }
            Debug.Log("Funny floaintg time" + speedMultiplier);

            //Ctrl k u to unncoment
        }
        else
        {
            speedMultiplier.y = 1;
        }
    }

    private void CollisionChecks()
    {
        bool top = CheckTopSides();
        bool bottom = CheckBottomSides();
        CheckGround();
        isTouchWall = top & bottom;

    }

    private bool CheckGround ()
    {
        RaycastHit2D hitLeft, hitRight;
        Collider2D playerCollider = GetComponent<Collider2D>();
        float collExtX = playerCollider.bounds.extents.x;
        float collExtY = playerCollider.bounds.extents.y;
        float distance = collExtY * 0.45f;
        float rayStartingPos = pos.y - (collExtY * 0.85f);

        //Two rays on each side

        Debug.DrawRay(new Vector3(pos.x - collExtX * 0.95f, rayStartingPos, 0), new Vector3(0,-distance,0), new Color(100,150,100));
        Debug.DrawRay(new Vector3(pos.x + collExtX * 0.95f, rayStartingPos, 0), new Vector3(0,-distance,0), new Color(100, 150, 100));
        hitLeft = Physics2D.Raycast(new Vector3(pos.x - collExtX * 0.95f, rayStartingPos, 0), Vector3.down, distance);
        hitRight = Physics2D.Raycast(new Vector3(pos.x + collExtX * 0.95f, rayStartingPos, 0), Vector3.down, distance);

        //Attach to touched Ground

        if (hitLeft.collider != null || hitRight.collider != null)
        {
            //Ground
            isGround = true;
            //TODO: Set x position to point of contact + height
            RaycastHit2D hit;
            if (hitLeft.collider != null)
            {
                hit = hitLeft;
            } else if (hitRight.collider != null)
            {
                hit = hitRight;
            } else
            {
                Debug.Log("THIS SHOULDN'T BE POSSIBLE");
                hit = new RaycastHit2D();
            }

            Vector2 updatePos = new Vector2(pos.x, pos.y);
            updatePos.y = hit.point.y + collExtY;

            //Debug.Log(" ||GROUND STUFF|| Ray has hit" + hit.point + " Player pos is " + pos + " Updating to " + updatePos);

            pos = updatePos;

            return true;
        } else
        {
            //Air
            isGround = false;
            return false;
        }

    }

    //TODO: Always entering here when touching ground try to fix this
    private bool CheckTopSides()
    {
        RaycastHit2D leftTopHit, rightTopHit;
        Collider2D playerCollider = GetComponent<Collider2D>();
        float collExtX = playerCollider.bounds.extents.x;
        float collExtY = playerCollider.bounds.extents.y;
        //Length of ray
        float distanceX = collExtX + collExtX / 95;
        float distanceY = collExtY + collExtY / 99;

        //Ray then debugDraw
        leftTopHit = Physics2D.Raycast(new Vector2(pos.x,pos.y + collExtY * 0.95f),Vector2.down, -distanceX);
        Debug.DrawRay(new Vector3(pos.x,pos.y+collExtY * 0.95f),new Vector3(-distanceX, 0));
        rightTopHit = Physics2D.Raycast(new Vector2(pos.x, pos.y + collExtY * 0.95f), Vector2.down, +distanceX);
        Debug.DrawRay(new Vector3(pos.x, pos.y + collExtY * 0.95f), new Vector3(distanceX, 0));

        RaycastHit2D hit = new RaycastHit2D();

        if (leftTopHit.collider != null)
        {
            hit = leftTopHit;
        }
        else if (rightTopHit.collider != null)
        {
            hit = rightTopHit;
        }

        if (hit.collider != null)
        {
            Vector2 updatePos = new Vector2(pos.x, pos.y);
            updatePos.x = hit.point.x;
            Debug.Log("Touched wall at " + hit.point + " teleporting to " + updatePos);
            isTouchWall = true;
            return true;
        } else
        {
            return false;
        }

        //Touching wall var?

        return true;
    }

    private bool CheckBottomSides()
    {
        RaycastHit2D leftBottomHit, rightBottomHit;
        Collider2D playerCollider = GetComponent<Collider2D>();
        float collExtX = playerCollider.bounds.extents.x;
        float collExtY = playerCollider.bounds.extents.y;
        //Length of ray
        float distanceX = collExtX + collExtX / 95;
        float distanceY = collExtY + collExtY / 99;

        leftBottomHit = Physics2D.Raycast(new Vector2(pos.x, pos.y - collExtY * 0.7f), Vector2.left, distanceX);
        Debug.DrawRay(new Vector3(pos.x, pos.y - collExtY * 0.7f), new Vector3(-distanceX, 0));
        rightBottomHit = Physics2D.Raycast(new Vector2(pos.x, pos.y - collExtY * 0.7f), Vector2.right, distanceX);
        Debug.DrawRay(new Vector3(pos.x, pos.y - collExtY * 0.7f), new Vector3(distanceX, 0));

        RaycastHit2D hit = new RaycastHit2D();

        if (leftBottomHit.collider != null)
        {
            hit = leftBottomHit;
        }
        else if (rightBottomHit.collider != null)
        {
            hit = rightBottomHit;
        }

        if (hit.collider != null)
        {
            speed.x = 0;
            Vector2 updatePos = new Vector2(pos.x, pos.y);
            updatePos.x = hit.point.x;
            Debug.Log("Touched wall at " + hit.point + " teleporting to " + updatePos);
            isTouchWall = true;
            return true;
        }
        else
        {
            return false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

}
