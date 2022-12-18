using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] GameObject weapon,reward,bananaManager;
    [SerializeField] float fireFrequency;
    float timeSinceFire;
    float timeAlive;
    string laserDirection;

    // Start is called before the first frame update
    void Start()
    {
        timeAlive = 0;
        switch (Random.Range(0,2))
        {
            case 0:
                laserDirection = "left";
                break;
            case 1:
                laserDirection = "right";
                break;
        }
        fireFrequency = Random.Range(fireFrequency - 1, fireFrequency + 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        SumTimeToFire();
        FireWeapon();
        timeAlive += Time.deltaTime;
        CheckDeath();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Touched player");
            PlayerHP playerHP = collision.gameObject.GetComponent<PlayerHP>();
            if (playerHP.HitByDamage(1))
            {
                Debug.Log("Damaged player by 1");
            }
            else
            {
                Debug.Log("Couldn't damage player");
            }
        }
    }

    private void FireWeapon()
    {
        if (timeSinceFire > fireFrequency)
        {
            timeSinceFire = 0;
            GameObject laser = Instantiate(weapon);
            laser.transform.position = new Vector2(this.transform.position.x,this.transform.position.y);
            laser.transform.parent = this.gameObject.transform; //Set the laser to child of enemy
            //Ignore collisions between the parent and the child
            Physics2D.IgnoreCollision(laser.GetComponent<BoxCollider2D>(), this.gameObject.GetComponent<BoxCollider2D>());
            //Get script and fire laser in direction
            LaserScript ls = laser.GetComponent<LaserScript>();
            ls.SetDirection(laserDirection);
        }
    }

    private void SumTimeToFire()
    {
        timeSinceFire += Time.deltaTime;
    }

    private void CheckDeath()
    {
        if (timeAlive > 6)
        {
            GameObject bananaManager = GameObject.Find("BananaManager");
            GameObject newReward = Instantiate(reward);
            newReward.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.5f); //Set pos Where enemy died
            newReward.transform.parent = bananaManager.transform; //Set bananaManager as parent
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Gaming4 playerScript = collision.gameObject.transform.parent.GetComponent<Gaming4>();
            playerScript.StompedEnemy();
            timeAlive = 7; //Change this
            CheckDeath();
        }
    }
}
