using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckScript : MonoBehaviour
{

    public bool IsTouchingWall { get; set;}
    float timeSinceTouchWall;
    bool isCountingTime;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceTouchWall = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CountTime();
        CheckMaxTime();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) //If it's the ground/wall
        {
            timeSinceTouchWall = 0;
            IsTouchingWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isCountingTime = true; //Count time since exited wall
        }
    }

    private void CountTime()
    {
        if (isCountingTime)
        {
            timeSinceTouchWall += Time.deltaTime;
        }
    }

    private void CheckMaxTime()
    {
        if (timeSinceTouchWall > 0.15f)
        {
            isCountingTime = false;
            timeSinceTouchWall = 0;
            IsTouchingWall = false;
        }
    }
}
