using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManagerScript : MonoBehaviour
{

    [SerializeField] GameObject enemy;
    [SerializeField] GameObject spawnPoints;
    [SerializeField] float maxSpawnTime, minSpawnTime;
    [SerializeField] private float spawnTime,nextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = 0;
        nextSpawnTime = RandomTime();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        SumTime();
        CheckTime();
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
        int randomChild = Random.Range(0, spawnPointCount - 1);
        Vector2 spawnPos = spawnPoints.transform.GetChild(randomChild).transform.position; //Get pos of spawnpoint
        //Create enemy
        GameObject spawnedEnemy = Instantiate(enemy);
        //Set pos
        spawnedEnemy.transform.position = spawnPos;

    }

    //Generates random time between min and max
    private float RandomTime()
    {
        return Random.Range(minSpawnTime, maxSpawnTime);
    }
}
