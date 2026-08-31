using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance { get; private set; }

    [Header("Spawn Setup")]
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject zombiePrefab;

    [Header("Wave Settings")]
    [SerializeField] private int zombiesPerWave = 5;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private int maxWaves = 8; // BARU: batas wave, sesi "menang" kalau tercapai

    [Header("DDA / Difficulty Presets")]
    [SerializeField] private DifficultyPreset[] difficultyPresets; // index 0=Easy, 1=Normal, 2=Hard
    private DifficultyPreset currentPreset; // BARU: preset yang lagi aktif, diset dari DDAController

    private int currentWave = 0;
    private int zombiesRemainingToSpawn;
    private int zombiesAlive;
    private int totalZombiesKilled = 0; // BARU: akumulasi SELURUH sesi (beda dari zombiesAlive yang per-wave)

    // BARU: expose read-only ke luar — dibutuhin UI end-screen & GameplayLogger nanti, tanpa buka akses tulis dari luar
    public int CurrentWave => currentWave;
    public int TotalZombiesKilled => totalZombiesKilled;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // BARU: validasi spawnPoints sebelum mulai apapun
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogError("WaveSpawner: spawnPoints kosong! Isi minimal 1 di Inspector.");
            return; // stop di sini, jangan lanjut StartNextWave() yang bakal crash
        }
        
    }

    public void BeginFirstWave() // BARU
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

    public void ApplyDifficultyPreset(int level)
    {
        if (difficultyPresets == null || level < 0 || level >= difficultyPresets.Length)
        {
            Debug.LogWarning("ApplyDifficultyPreset: level di luar range, pakai index 1 (Normal).");
            level = 1;
        }

        currentPreset = difficultyPresets[level]; // BARU
        zombiesPerWave = currentPreset.zombieCount;
        spawnInterval = currentPreset.spawnInterval;
        Debug.Log($"Preset diterapkan: level={level}, health={currentPreset.zombieHealth}, speed={currentPreset.zombieSpeed}");
    }

    void SpawnZombie()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject zombieObj = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation); // BARU: simpen hasil Instantiate

        if (currentPreset != null)
        {
            Zombie zombie = zombieObj.GetComponent<Zombie>(); // BARU: ambil komponen Zombie dari object yang baru dibuat
            zombie.SetStats(currentPreset.zombieHealth, currentPreset.zombieSpeed);
        }

        zombiesAlive++;
    }

    private bool waveTransitionInProgress = false; // lapisan kedua, backstop kalau Zombie.cs kelewat kena bug lama

    public void OnZombieDied()
    {
        zombiesAlive--;
        totalZombiesKilled++;

        if (waveTransitionInProgress) return; // wave ini udah diproses, abaikan panggilan susulan

        if (zombiesAlive <= 0 && zombiesRemainingToSpawn <= 0)
        {
            waveTransitionInProgress = true;
            GameplayLogger.Instance.LogWave(currentWave);

            bool isLastWave = currentWave >= maxWaves;
            WaveTransitionUI.Instance.OnWaveComplete(currentWave, () =>
            {
                waveTransitionInProgress = false; // reset buat wave berikutnya
                if (isLastWave)
                    GameManager.Instance.EndGame(true);
                else
                    StartNextWave();
            });
        }
    }
}