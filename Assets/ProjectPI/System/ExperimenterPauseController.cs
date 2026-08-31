using UnityEngine;

public class ExperimenterPauseController : MonoBehaviour
{
    [SerializeField] private GameObject transitionOverlay; // reuse UI wave-transition kalau ada
    [SerializeField] private KeyCode pauseKey = KeyCode.F9; // hotkey tersembunyi, gak ada di UI player
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(pauseKey)) TogglePause();
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        transitionOverlay.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        if (!isPaused)
            DDAController.Instance.ReloadDifficulty(); // baca ulang file SEBELUM wave 4 mulai
    }
}