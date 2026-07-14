using UnityEngine;

// [CreateAssetMenu] bikin ini muncul di menu klik-kanan > Create > DDA
[CreateAssetMenu(fileName = "NewDifficultyPreset", menuName = "DDA/Difficulty Preset")]
public class DifficultyPreset : ScriptableObject
{
    [Header("Zombie Wave Settings")]
    public int zombieCount = 5;       // jumlah zombie per wave
    public float zombieSpeed = 3.5f;  // dipakai NavMeshAgent.speed
    public float zombieHealth = 100f;
    public float spawnInterval = 2f;  // jeda antar spawn zombie (detik)
}