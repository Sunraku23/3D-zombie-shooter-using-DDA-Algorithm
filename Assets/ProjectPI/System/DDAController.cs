using UnityEngine;
using System.IO;

[System.Serializable]
public class DDAResult
{
    public int difficulty_level; // 0 = easy, 1 = normal, 2 = hard
}

public class DDAController : MonoBehaviour
{
    public static DDAController Instance;
    void Awake() => Instance = this;

    private void Start()
    {
        Debug.Log("Persistent path: " + Application.persistentDataPath);
        ReloadDifficulty();
    }

    public void ReloadDifficulty()
    {
        int level = 1; // fallback Normal
        try
        {
            string path = Path.Combine(Application.persistentDataPath, "dda_output.json");
            var data = JsonUtility.FromJson<DDAResult>(File.ReadAllText(path));
            level = data.difficulty_level;
        }
        catch (FileNotFoundException)
        {
            Debug.Log("dda_output.json belum ada, pakai Normal.");
        }
        WaveSpawner.Instance.ApplyDifficultyPreset(level);
    }

    [ContextMenu("TEST: Reload Difficulty")] // buat step 2 testing kemarin
    private void TestReload() => ReloadDifficulty();
}