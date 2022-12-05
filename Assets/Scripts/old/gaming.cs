using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class gaming : MonoBehaviour
{

    public float jumpVelocity;
    public Vector2 pos;
    public Vector2 vel;
    public Vector2 accel;
    public float sideAccel;
    public float gravity;
    public bool isGravity;
    private int funnyNumber;
    public bool onGround;
    public float maxVel;
    public float frictionMultiplier;
    new BoxCollider2D collider;

    private Vector2 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
        pos = gameObject.transform.position;
        //Defaults
        gravity = -9.81f;
        jumpVelocity = -gravity*2;
        isGravity = false;
        sideAccel = 60;
        onGround = true;
        maxVel = 15;
        frictionMultiplier = 2;
        //ObjectStuff
        pos = new Vector2(transform.position.x, transform.position.y);
        lastPos = pos;
        vel = new Vector2(0, 0);
        accel = new Vector2(0, 0);
        collider = GetComponent<BoxCollider2D>();

        funnyNumber = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //funnyRay();
        ToggleGravity();
    }

    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            vel.y = jumpVelocity;
            Debug.Log("w");
            onGround = false; //CHANGE THIS WHEN COLLISION ARE DONE
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (vel.x < 0)
            {
                accel.x = sideAccel * frictionMultiplier;
            } else
            {
                accel.x = sideAccel;
            }

        }

        if (Input.GetKey(KeyCode.A))
        {
            if (vel.x > 0)
            {
                accel.x = -sideAccel * frictionMultiplier;
            } else
            {
                accel.x = -sideAccel;
            }
        }

    }

    private void ReduceAccel()
    {
        //Gravity
        if (isGravity && !onGround)
        {
            accel.y = gravity;
        }
        //Friction
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (vel.x > 0) //if going right
            {
                accel.x = -sideAccel*frictionMultiplier;
            } else if (vel.x < 0) //if going left
            {
                accel.x = sideAccel*frictionMultiplier;
            }

        }
    }

    private void MovementStuff()
    {
        //Set vel
        vel.x += accel.x * Time.deltaTime;
        vel.y += accel.y * Time.deltaTime;

        if (vel.x > maxVel)
        {
            vel.x = maxVel;
        } else if (vel.x < -maxVel)
        {
            vel.x = -maxVel;
        }

        if (vel.y > jumpVelocity)
        {
            vel.y = jumpVelocity;
        } else if (vel.y < -jumpVelocity)
        {
            //Correct if ground collision
            //vel.y = -maxVel;
            //Debug while no collision CHANGE WHEN COLLISION IS DONE
            vel.y = 0;
            onGround = true;
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) //Slow down to exact 0 when no keys pressed
        {
            if ((accel.x > 0 && vel.x > 0) || (accel.x < 0 && vel.x < 0))
            {
                accel.x = 0;
                vel.x = 0;
            }
        }

        //Set pos
        pos.x += vel.x * Time.deltaTime;
        pos.y += vel.y * Time.deltaTime;

    }

    private void ToggleGravity()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isGravity)
            {
                isGravity = false;
                vel.y = 0;
                accel.y = 0;
                Debug.Log("adeuuu gravity");
            }
            else
            {
                isGravity = true;
                Debug.Log("hello gravity");
            }
        }
    }

    private void CheckGround()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(pos, Vector2.down, collider.bounds.extents.y-0.1f);
        Debug.DrawRay(pos, new Vector2(0,-collider.bounds.extents.y-0.1f), new Color(100,150,100));
        Debug.Log(rayHit.collider.ToString());
    }

    private void setPosition()
    {
        //Set transform to new
        gameObject.transform.position = pos;
        //Store data for next frame
        lastPos.x = pos.x;
        lastPos.y = pos.y;
    }

    private void funnyRay()
    {
        funnyNumber++;
        Vector3 funnyVector = new Vector3(pos.x,pos.y,0);
        Vector3 finalVector = new Vector3((float)(pos.x * 200 * Math.Cos(funnyNumber)), (float)(pos.y * 200 * Math.Sin(funnyNumber)), 0);
        Debug.DrawLine(funnyVector, finalVector, new Color(100f,50f,100f), 2);
        if (funnyNumber == 360)
        {
            funnyNumber = 0;
        }
        print(funnyNumber);
        print(funnyVector);
        print(finalVector);
    }

    private void funnyRotate()
    {
        Transform transformReal = GetComponent<Transform>();
        Quaternion target = Quaternion.Euler(0, 0, transformReal.rotation.z + vel.x * Time.deltaTime);
        transform.rotation = target;

    }

    private void FixedUpdate()
    {
        CheckGround();
        CheckInputs();
        ReduceAccel();
        MovementStuff();
        setPosition();
    }

}
