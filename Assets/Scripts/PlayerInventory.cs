using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    Dictionary<string, int> inventory;
    ArrayList collectibleNames;
    [SerializeField] TextMeshProUGUI textUi;

    private void Start()
    {
        inventory = new Dictionary<string, int>();
        collectibleNames = new ArrayList();
    }

    private void Update()
    {

    }

    private void UpdateText()
    {
        string newText = "";
        foreach (var item in inventory)
        {
            newText += "    : " + item.Value + "\n";
        }
        textUi.text = newText;
    }

    private void SetCollectibleNames()
    {
        collectibleNames.Add("Banana");
        inventory.Add("Banana", 0);
    }

    public void Collect (string collectible)
    {
        collectibleNames.Add(collectible);
        if (!inventory.ContainsKey(collectible))
        {
            inventory.Add(collectible, 0);
        }
        inventory[collectible]++;
        PrintCollectibles();
        UpdateText();
    }

    private void PrintCollectibles()
    {
        string result = "";

        foreach (string collectible in collectibleNames)
        {
            result += collectible + inventory[collectible] + ",";
        }

        Debug.Log(result);
    }

    public Dictionary<string,int> getInventory()
    {
        return inventory;
    }
}
