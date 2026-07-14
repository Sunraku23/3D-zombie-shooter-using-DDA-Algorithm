using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // === Singleton ===
    public static WaveSpawner Instance { get; private set; }

    [Header("Spawn Setup")]
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject zombiePrefab;

    [Header("Wave Settings")]
    [SerializeField] private int zombiesPerWave = 5;
    [SerializeField] private float spawnInterval = 1.5f; // jeda antar spawn dalam 1 wave

    private int currentWave = 0;
    private int zombiesRemainingToSpawn;
    private int zombiesAlive;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Udah ada WaveSpawner lain duluan -> ini duplikat, hancurin
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWave++;
        zombiesRemainingToSpawn = zombiesPerWave;
        zombiesAlive = 0;

        Debug.Log("Wave " + currentWave + " dimulai! Total zombie: " + zombiesPerWave);

        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        while (zombiesRemainingToSpawn > 0)
        {
            SpawnZombie();
            zombiesRemainingToSpawn--;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnZombie()
    {
        // Pilih spawn point secara acak dari list
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
        zombiesAlive++;
    }

    // Dipanggil dari ZombieAI.cs pas zombie itu mati
    public void OnZombieDied()
    {
        zombiesAlive--;

        // Kalau semua zombie di wave ini udah mati DAN gak ada lagi yang nunggu spawn -> wave selesai
        if (zombiesAlive <= 0 && zombiesRemainingToSpawn <= 0)
        {
            StartNextWave();
        }
    }
}