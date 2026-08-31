using System.Collections;
using TMPro;
using UnityEngine;

public class WaveTransitionUI : MonoBehaviour
{
    public static WaveTransitionUI Instance; // BARU: tadinya gak ada, makanya "Instance" error
    void Awake() => Instance = this;

    [SerializeField] private GameObject transitionPanel;
    [SerializeField] private TMP_Text waveCompleteText;
    [SerializeField] private float normalTransitionDuration = 2.5f;
    [SerializeField] private int ddaCheckpointWave = 3;
    [SerializeField] private KeyCode experimenterResumeKey = KeyCode.F9;

    // BARU: terima onComplete, biar WaveSpawner bisa nyuruh "lanjut wave berikutnya" SETELAH panel bener-bener nutup
    public void OnWaveComplete(int waveNumber, System.Action onComplete)
    {
        StartCoroutine(HandleTransition(waveNumber, onComplete));
    }

    private IEnumerator HandleTransition(int waveNumber, System.Action onComplete)
    {
        transitionPanel.SetActive(true);
        waveCompleteText.text = $"Wave {waveNumber} Complete!";
        Time.timeScale = 0f;

        if (waveNumber == ddaCheckpointWave)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(experimenterResumeKey));
            DDAController.Instance.ReloadDifficulty();
        }
        else
        {
            yield return new WaitForSecondsRealtime(normalTransitionDuration);
        }

        transitionPanel.SetActive(false);
        Time.timeScale = 1f;

        onComplete?.Invoke(); // WAJIB paling akhir — wave berikutnya baru mulai setelah panel tertutup
    }
}