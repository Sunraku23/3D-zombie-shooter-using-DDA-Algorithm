using UnityEngine;
using System.IO;

[System.Serializable]
public class DDAResult
{
    public int difficulty_level; // 0 = easy, 1 = normal, 2 = hard
}

public class DDAController : MonoBehaviour
{
    [SerializeField] private WaveSpawner waveSpawner;



    private DDAResult LoadDDAResult()
    {
        string path = Application.persistentDataPath + "/dda_output.json";

        try
        {
            string json = File.ReadAllText(path);
            DDAResult result = JsonUtility.FromJson<DDAResult>(json);
            return result;
        }
        catch (FileNotFoundException)
        {
            Debug.LogWarning("dda_output.json tidak ditemukan, pakai preset Normal default.");
            return new DDAResult { difficulty_level = 1 }; // 1 = normal
        }

    }

    private void Start()
    {
        DDAResult result = LoadDDAResult();
        ApplyDifficulty(result.difficulty_level);
        Debug.Log(Application.persistentDataPath);
    }



    private void ApplyDifficulty(int level)
    {
        // TODO: suruh waveSpawner ganti preset index
        waveSpawner.ApplyDifficultyPreset(level);
    }
}