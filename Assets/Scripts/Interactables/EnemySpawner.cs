using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float numberOfEnemiesToSpawn;
    //Interval between waves after a wave has cleared
    public float timeBetweenSpawns;
    //Interval Between each spawned enemy per wave
    public float timeBetweenEnemies;

    public int totalWaves;
    private int currentWave;
    public bool WaveCleared = false;
    public bool PlayerEntered = false;
    public float spawnRadius;

    public GameObject enemyPrefab;
    
    
    public List<GameObject> spawnedEnemies;
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => PlayerEntered);
        timeBetweenEnemies = Conductor.Instance.secondsPerBeat;
        StartCoroutine(SpawnWave());
    }


    private IEnumerator SpawnWave()
    {
        if (currentWave == 0)
        {
            while (spawnedEnemies.Count <= numberOfEnemiesToSpawn)
                
            {
                yield return new WaitForSeconds(timeBetweenEnemies);
                SpawnEnemies();
            }

            currentWave++;

        }
        yield return new WaitUntil(() => WaveCleared);
        
        InvokeRepeating(nameof(SpawnEnemies), timeBetweenSpawns, timeBetweenEnemies);
    }

    private void SpawnEnemies()
    {
        var spawnPosition = new Vector3(Random.Range(transform.position.x - spawnRadius, transform.position.x + spawnRadius), transform.position.y,Random.Range(transform.position.z - spawnRadius, transform.position.z + spawnRadius));
        var enemySpawned = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemies.Add(enemySpawned);
    }

    public void SetPlayerEntered()
    {
        PlayerEntered = true;
    }
}