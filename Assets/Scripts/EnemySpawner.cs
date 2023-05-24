using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnInterval;
    private Transform playerTransform;
    private float spawnTimer = 0;

    void Start() {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update() {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval) {
            SpawnEnemy();
            spawnTimer = 0;
        }
    }

    public void SetSpawnInterval(float spawnIntervalIncrease) {
        spawnInterval -= spawnIntervalIncrease;
    }

    void SpawnEnemy() {
        Vector2 randomOffset = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

}
