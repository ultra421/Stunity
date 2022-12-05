using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaCollect : MonoBehaviour 
{
    [SerializeField] public string collectName;

    private void Start()
    {
        collectName = "Banana";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If collision has playerInventory component it's the player
        PlayerInventory inventory = collision.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.Collect(collectName);
            Destroy(gameObject);
            Debug.Log("Got " + collectName);
        }

    }
}
