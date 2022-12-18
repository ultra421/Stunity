using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    //This script manages player's HP and their invulnerability frames

    [SerializeField] public int playerHP, invulFrames, maxInvulTime;
    [SerializeField] TextMeshProUGUI textUI;
    // Start is called before the first frame update
    void Start()
    {
        
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

}
