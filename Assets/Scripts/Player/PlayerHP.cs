using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    //This script manages player's HP and their invulnerability frames

    [SerializeField] public int playerHP, invulFrames, maxInvulTime;
    [SerializeField] TextMeshProUGUI textUI;
    Animator animator;
    float timeSinceHit;
    bool gotHit;
    // Start is called before the first frame update
    void Start()
    {
       animator = GetComponent<Animator>();
        gotHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    private void FixedUpdate()
    {
        SumInvulFrame();
        CheckHP();
        hitAnim();
    }

    public bool HitByDamage(int damage)
    {
        if (IsInvulnerable())
        {
            return false;
        } else
        {
            invulFrames = 1;
            Debug.Log("InvulFrame = " + invulFrames);
            playerHP -= damage;
            animator.SetBool("gotHit", true);
            gotHit = true;
            return true;
        }
    }

    private void CheckHP()
    {
        if (playerHP == 0)
        {
            Debug.Log("Ded, not big soup rise");
            Destroy(this.gameObject);
        }
    }

    private void UpdateUI()
    {
        textUI.text = "    : " + playerHP;
    }

    public bool IsInvulnerable()
    {
        if (invulFrames < maxInvulTime)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void SumInvulFrame()
    {
        if (invulFrames < maxInvulTime)
        {
            invulFrames++;
        }
    }

    private void hitAnim()
    {
        if (gotHit)
        {
            timeSinceHit += Time.deltaTime;
        }

        if (timeSinceHit > 1)
        {
            timeSinceHit = 0;
            gotHit = false;
            animator.SetBool("gotHit", gotHit);
        }
    }

}
