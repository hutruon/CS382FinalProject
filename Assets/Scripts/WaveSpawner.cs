using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public WaveData[] waves;
    public int curWave = 0;
    public int wavesPerLevel = 5; // Number of waves per level
    private int wavesSpawnedThisLevel = 0; // Counter for waves spawned in the current level

    public float baseEnemySpeed = 1.0f; // Base speed of enemies
    private float currentEnemySpeed; // Current speed of enemies for the wave

    public int remainingEnemies;

    [Header("Components")]
    public Transform enemySpawnPos;
    public TextMeshProUGUI waveText;
    public GameObject nextWaveButton;

    // Added unity event.
    [Header("Events")]
    public UnityEvent OnEnemyRemoved;

    void OnEnable()
    {
        Enemy.OnDestroyed += OnEnemyDestroyed;
    }

    void OnDisable()
    {
        Enemy.OnDestroyed -= OnEnemyDestroyed;
    }

    public void SpawnNextWave()
    {
        if (curWave >= waves.Length || wavesSpawnedThisLevel >= wavesPerLevel) // Check if reached end of waves or waves per level
            return;

        curWave++;
        waveText.text = $"Wave: {curWave}";
        currentEnemySpeed = baseEnemySpeed + (curWave * 0.3f); // Increase speed with each wave
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        nextWaveButton.SetActive(false);
        WaveData wave = waves[curWave - 1];

        for (int x = 0; x < wave.enemySets.Length; x++)
        {
            yield return new WaitForSeconds(wave.enemySets[x].spawnDelay);

            for (int y = 0; y < wave.enemySets[x].spawnCount; y++)
            {
                SpawnEnemy(wave.enemySets[x].enemyPrefab);
                yield return new WaitForSeconds(wave.enemySets[x].spawnRate);
            }
        }

        wavesSpawnedThisLevel++; // Increment the counter for waves spawned this level
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemyInstance = Instantiate(enemyPrefab, enemySpawnPos.position, Quaternion.identity);
        enemyInstance.GetComponent<Enemy>().SetSpeed(currentEnemySpeed); // Set the speed of the spawned enemy
        remainingEnemies++;
    }

    void OnEnemyDestroyed()
    {
        remainingEnemies--;

        if (remainingEnemies == 0)
            nextWaveButton.SetActive(true);

        // Added invokation of unity event.
        OnEnemyRemoved?.Invoke();
    }
}
