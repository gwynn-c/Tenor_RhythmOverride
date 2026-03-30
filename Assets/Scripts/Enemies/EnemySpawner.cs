using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public float numberOfEnemiesToSpawn;
    public int enemyKilled;
    //Interval between waves after a wave has cleared
    public float timeBetweenSpawns;
    //Interval Between each spawned enemy per wave
    public float timeBetweenEnemies;

    public TextMeshPro enemyCountText;
    public TextMeshPro waveCountText;
    public TextMeshPro enemyNameText;

    public UnityEvent completeLevel; 
    
    public int totalWaves;
    private int currentWave;
    public bool WaveCleared = false;
    public bool PlayerEntered = false;
    public float spawnRadius;

    public GameObject[] enemyPrefab;
    public int currentEnemyIndex;
    public List<GameObject> spawnedEnemies;
    public void OnEnable() => EventManager.instance.playerEvents.EnemyKilled += CheckEnemyState;

    private void CheckEnemyState()
    {
        enemyKilled++;
        
    }

    public void OnDisable() => EventManager.instance.playerEvents.EnemyKilled -= CheckEnemyState;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => PlayerEntered);
        timeBetweenEnemies = Conductor.Instance.secondsPerBeat;
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if (enemyKilled >= numberOfEnemiesToSpawn)
        {
            completeLevel?.Invoke();
            
        }
        if (SceneManager.GetActiveScene().name != "Playground") return;
        enemyCountText.SetText(numberOfEnemiesToSpawn.ToString());
        waveCountText.SetText(currentWave.ToString());
        enemyNameText.SetText(enemyPrefab[currentEnemyIndex].name);

        
    }
    private IEnumerator SpawnWaves()
    {
        if (currentWave == 0)
        {
            while (spawnedEnemies.Count < numberOfEnemiesToSpawn)
                
            {
                yield return new WaitForSeconds(timeBetweenEnemies);
                SpawnEnemies();
            }
    
            currentWave++;
    
        }
        yield return new WaitUntil(() => WaveCleared);
        
        Invoke(nameof(SpawnEnemies), timeBetweenSpawns);
    }

    private void SpawnEnemies()
    {
        var spawnPosition = new Vector3(Random.Range(transform.position.x - spawnRadius, transform.position.x + spawnRadius), transform.position.y,Random.Range(transform.position.z - spawnRadius, transform.position.z + spawnRadius));
        var enemySpawned = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length-1)], spawnPosition, Quaternion.identity);
        spawnedEnemies.Add(enemySpawned);
    }
    private void SpawnEnemy()
    {
        var spawnPosition = new Vector3(Random.Range(transform.position.x - spawnRadius, transform.position.x + spawnRadius), transform.position.y,Random.Range(transform.position.z - spawnRadius, transform.position.z + spawnRadius));
        var enemySpawned = Instantiate(enemyPrefab[currentEnemyIndex], spawnPosition, Quaternion.identity);
        spawnedEnemies.Add(enemySpawned);
    }

    public void SetPlayerEntered()
    {
        PlayerEntered = true;
    }

    public void IncreaseEnemiesToSpawn()
    {
        numberOfEnemiesToSpawn++;
    }

    public void DecreaseEnemiesToSpawn()
    {
        if(numberOfEnemiesToSpawn > 0)
            numberOfEnemiesToSpawn--;
    }
    public void SpawnWave()
    {
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            SpawnEnemy();
        }
    }

    public void ChangeEnemy()
    {
        if (currentEnemyIndex >= enemyPrefab.Length - 1)
        {
            currentEnemyIndex = 0;
        }
        else
        {
            currentEnemyIndex++;
        }
    }
}