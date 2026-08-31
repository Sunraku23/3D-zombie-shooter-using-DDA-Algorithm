using System.IO;
using UnityEngine;

public class GameplayLogger : MonoBehaviour
{
    public static GameplayLogger Instance;

    private string filePath;
    private ThirdPersonshootercontroller playerShooter; // referensi ke accuracy counter
    private PlayerHealth playerHealth;
    private string csvPath;

    void Awake()
    {
        Instance = this;
        filePath = Application.persistentDataPath + "/gameplay_log.csv";
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerShooter = player.GetComponent<ThirdPersonshootercontroller>();
        playerHealth = player.GetComponent<PlayerHealth>();

        csvPath = Path.Combine(Application.persistentDataPath, "gameplay_log.csv");
        // header ditulis sekali tiap sesi mulai — timpa file lama (lihat catatan di bawah)
        File.WriteAllText(filePath, "wave,kills_per_minute,accuracy,health_remaining,deaths,time_survived\n");
        //Debug.Log("Log file location: " + filePath);
    }

    public void LogWave(int waveNumber)
    {
        int totalKills = WaveSpawner.Instance.TotalZombiesKilled;
        float timeSurvived = GameManager.Instance.TimeSurvived;
        float killsPerMinute = totalKills / (Mathf.Max(timeSurvived, 0.1f) / 60f);
        float accuracy = playerShooter.ShotsFired > 0 ? (float)playerShooter.ShotsHit / playerShooter.ShotsFired : 0f;
        int healthRemaining = playerHealth.CurrentHealth;
        int deaths = playerHealth.IsDead ? 1 : 0;

        string row = $"{waveNumber},{killsPerMinute:F2},{accuracy:F2},{healthRemaining},{deaths},{timeSurvived:F1}";

        try
        {
            File.AppendAllText(filePath, row + "\n");
            Debug.Log("Logged: " + row);
        }
        catch (IOException e)
        {
            Debug.LogError("Gagal nulis log wave " + waveNumber + " (file lagi dipake program lain): " + e.Message);
            // sengaja gak di-throw ulang — 1 baris data hilang lebih baik ketimbang seluruh sesi freeze
        }
    }
}