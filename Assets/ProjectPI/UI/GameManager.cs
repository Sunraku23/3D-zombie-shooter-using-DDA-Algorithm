using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState { MainMenu, Playing, GameOver }
    private GameState currentState;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText; // BARU — drag Text (TMP) di dalam GameOverPanel

    private float gameStartTime; // BARU



    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetState(GameState.MainMenu);

        // BARU — subscribe ke event kematian player, sama pattern kayak Zombie.cs cari PlayerHealth
        PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerHealth.OnPlayerDied += () => EndGame(false);
    }

    void SetState(GameState newState)
    {
        currentState = newState;

        mainMenuPanel.SetActive(newState == GameState.MainMenu);
        crosshair.SetActive(newState == GameState.Playing);
        gameOverPanel.SetActive(newState == GameState.GameOver);

        Time.timeScale = (newState == GameState.Playing) ? 1f : 0f;

        // BARU
        Cursor.visible = (newState != GameState.Playing);
        Cursor.lockState = (newState == GameState.Playing) ? CursorLockMode.Locked : CursorLockMode.None;
    }

    // BARU — satu pintu masuk buat "game selesai", entah menang atau kalah
    public void EndGame(bool won)
    {
        gameOverText.text = won ? "You Survived!" : "You Died";
        SetState(GameState.GameOver);
    }

    public void StartGame()
    {
        SetState(GameState.Playing);
        gameStartTime = Time.time; // BARU — catet jam mulai
        WaveSpawner.Instance.BeginFirstWave(); // BARU
    }

    public float TimeSurvived => Time.time - gameStartTime;

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}