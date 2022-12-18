using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManagerScript : MonoBehaviour
{

    [SerializeField] GameObject enemy, spawnPoints, player;
    [SerializeField] float maxSpawnTime, minSpawnTime;
    [SerializeField] private float spawnTime,nextSpawnTime;
    [SerializeField] TextMeshProUGUI gameOverText;
    bool[] occupiedPoints;
    float timeSinceLastPointReset;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = 0;
        nextSpawnTime = RandomTime();
        gameOverText.gameObject.SetActive(false);
        occupiedPoints = new bool[spawnPoints.transform.childCount];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        SumTime();
        CheckTime();
        resetPoints();
        //CheckGameOver();
    }

    private void SumTime()
    {
        if (spawnTime < maxSpawnTime)
        {
            spawnTime += Time.deltaTime;
        }
    }

    private void CheckTime()
    {
        if (spawnTime >= nextSpawnTime)
        {
            SpawnEnemy();
            spawnTime = 0;
            nextSpawnTime = RandomTime();
        }
    }

    private void SpawnEnemy()
    {
        int spawnPointCount = spawnPoints.transform.childCount;
        Debug.Log("Found points = " + spawnPointCount);

        int randomChild = 0;
        while (true)
        {
            randomChild = Random.Range(0, spawnPointCount - 1);
            if (occupiedPoints[randomChild])
            {
                continue;
            } else
            {
                break;
            }
        }
        Debug.Log("Using point = " + randomChild);
        Vector2 spawnPos = spawnPoints.transform.GetChild(randomChild).transform.position; //Get pos of spawnpoint
        //Create enemy
        GameObject spawnedEnemy = Instantiate(enemy);
        //Set pos
        spawnedEnemy.transform.position = spawnPos;
        spawnedEnemy.transform.SetParent(this.gameObject.transform);
        occupiedPoints[randomChild] = true;
    }

    //Generates random time between min and max
    private float RandomTime()
    {
        return Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void CheckGameOver()
    {
        PlayerHP hp = player.GetComponent<PlayerHP>();
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (hp.playerHP == 0)
        {
            gameOverText.text = "Has conseguido " + inventory.getQuantity("banana") + " bananas";
            gameOverText.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }

    private void resetPoints()
    {
        timeSinceLastPointReset += Time.deltaTime;
        if (timeSinceLastPointReset > 20) //reset the points
        {
            for (int i = 0; i < occupiedPoints.Length; i++)
            {
                occupiedPoints[i] = false;
            }
            timeSinceLastPointReset = 0;
        }
    }

}
