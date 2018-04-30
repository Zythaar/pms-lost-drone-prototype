using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerSpawn;
    public Transform enemySpawn;

    public Transform playerParent;
    public Transform enemyParent;

    public Vector2 enemySpawnRange;

    [Header("Spawning")]
    public bool isSpawning;

    public float spawnRate;
    private float nextSpawnTime;

    private void Start()
    {
        SpawnPlayer();
    }

    private void Update()
    {
        if (!isSpawning)
        {
            return;
        }


        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnRate;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        float random = Random.Range(enemySpawnRange.x, enemySpawnRange.y);
        Vector3 randomSpawnPos = enemySpawn.position + Vector3.right * random;
        Instantiate(enemyPrefab, randomSpawnPos, enemySpawn.rotation, enemyParent);
    }

    [ContextMenu("Respawn")]
    public void SpawnPlayer()
    {
        Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
    }

}
