using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { MainMenu, Playing, GameOver }
    private GameState currentState;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject gameOverPanel;

    void Awake()
    {
        Instance = this; // Singleton, sama kayak WaveSpawner
    }

    void Start()
    {
        SetState(GameState.MainMenu);
    }

    void SetState(GameState newState)
    {
        currentState = newState;

        mainMenuPanel.SetActive(newState == GameState.MainMenu);
        crosshair.SetActive(newState == GameState.Playing);
        gameOverPanel.SetActive(newState == GameState.GameOver);

        // freeze/unfreeze seluruh scene
        Time.timeScale = (newState == GameState.Playing) ? 1f : 0f;
    }

    public void StartGame() => SetState(GameState.Playing); // dipanggil tombol Start

    public void RestartGame()
    {
        Time.timeScale = 1f; // WAJIB reset dulu sebelum reload, atau scene baru ikut freeze
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}