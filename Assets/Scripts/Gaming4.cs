using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaming4 : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] Vector2 sideSpeed;
    [SerializeField] Vector2 jumpSpeed;
    bool jumpInput;
    bool isGround;
    [SerializeField] Vector2 vel,maxVel;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vel = rb.velocity;
        maxVel = new Vector2(5, 100);
        sideSpeed = new Vector2(20,0);
        jumpSpeed = new Vector2(0,500);
        jumpInput = false;
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

        if (jumpInput && isGround)
        {
            Debug.Log("jumpin");
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
        //TODO: Remove
        rb.velocity = vel;
    }

    private void CheckGround()
    {
        //Start ray below player
        Vector2 pos = rb.position;
        BoxCollider2D bc2 = GetComponent<BoxCollider2D>();
        float rayDistance = 0.16f;
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
        }

    }
}
