using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] int laserVel;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject tileMap = GameObject.Find("mainTilemap");
        CompositeCollider2D compCollider = tileMap.GetComponent<CompositeCollider2D>();

        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), compCollider);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void SetDirection(string laserDirection)
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 velocity = rb.velocity;
        if (string.Equals(laserDirection, "right"))
        {
            velocity = new Vector2(laserVel, 0);
        }
        else if (string.Equals(laserDirection, "left"))
        {
            velocity = new Vector2(-laserVel, 0);
        }
        rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHP playerHP = collision.gameObject.GetComponent<PlayerHP>();
            playerHP.HitByDamage(1);
            Destroy(this.gameObject);
        }
    }

}
