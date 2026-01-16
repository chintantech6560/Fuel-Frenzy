using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { Playing, Paused, GameOver }
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI gameoverText;
    [SerializeField] private Button restartButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Slider fuelSlider;

    [Header("Pause System")]
    [SerializeField] public GameObject playButton;  
    [SerializeField] public GameObject pauseButton;

    [Header("Difficulty Scaling (Global Speed)")]
    public float globalSpeed = 2.0f;       
    public float speedIncreaseRate = 0.2f; 
    public float speedInterval = 5.0f;     
    private float speedTimer;

    [Header("Game States")]
    public GameState currentState;


    [Header("Fuel System")]
    [SerializeField]public float maxFuel = 100f;
    [SerializeField]public float currentFuel;
    [SerializeField]public float fuelDrainRate = 3f; 
    [SerializeField] public float fuelRefillAmount = 20f;

    [Header("Health System")]
    public int maxHealth = 3;
    public int currentHealth;

   
    private float score;

    void Awake()
    {
        if (instance == null) instance = this;
        Screen.orientation = ScreenOrientation.Portrait;
    }

    void Start()
    {
        score = 0;
        currentFuel = maxFuel;
        currentHealth = maxHealth;
        scoreText.text = "Score:" + score;

       // globalSpeed = 5.0f;
        speedTimer = 0f;

        // Init State (Game Start = Playing)
        SetGameState(GameState.Playing);

        Time.timeScale = 1f; 
        if (playButton != null) playButton.SetActive(false); 
        if (pauseButton != null) pauseButton.SetActive(true);

    }
    void Update()
    {
        if (currentState != GameState.Playing) return;
        currentFuel -= fuelDrainRate * Time.deltaTime;

        // Update the UI Slider
        if (fuelSlider != null)
        {
            fuelSlider.value = currentFuel / maxFuel;
        }
        if (currentFuel <= 0)
        {
            currentFuel = 0;
            TriggerGameOver("Fuel Empty!");
        }
        // globle speed Increase
        speedTimer += Time.deltaTime;
        if (speedTimer >= speedInterval)
        {
            globalSpeed += speedIncreaseRate; //Increse speed
            speedTimer = 0f; // Timer reset
        }

        score += globalSpeed * Time.deltaTime;
        scoreText.text = "Score:" + score.ToString("F0");

    }
    public void AddFuel()
    {
        if (currentState != GameState.Playing) return;
        currentFuel += fuelRefillAmount;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        Debug.Log("Fuel Refilled. Current: " + currentFuel);
    }
    public void TakeDamage(int damageAmount)
    {
        if (currentState != GameState.Playing) return;

        currentHealth -= damageAmount;
        UpdateHearts(currentHealth);
        Debug.Log("Health Lost! Remaining: " + currentHealth);
        if (currentHealth < 0)
        {
            currentHealth = 0;
            TriggerGameOver("Car Creshed..");
        }
    }
    public void SetGameState(GameState newState)
    {
        currentState = newState;

        if (currentState == GameState.Playing)
        {
            Time.timeScale = 1f;
            if (playButton != null) playButton.SetActive(false);
            if (pauseButton != null) pauseButton.SetActive(true);
        }
        else if (currentState == GameState.Paused)
        {
            Time.timeScale = 0f;
            if (pauseButton != null) pauseButton.SetActive(false);
            if (playButton != null) playButton.SetActive(true);
        }
        else if (currentState == GameState.GameOver)
        {
            Time.timeScale = 0f; // Sab rok do
            if (pauseButton != null) pauseButton.SetActive(false);
            // Play button mat dikhao game over par
            if (playButton != null) playButton.SetActive(false);
        }
    }
    void TriggerGameOver(string reason)
    {
        SetGameState(GameState.GameOver);
        gameoverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        // Debug.Log("GAME OVER: " + reason);
    }
    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentHealth;
        }
    }
    public void PauseGame()
    {
        SetGameState(GameState.Paused);
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        SetGameState(GameState.Playing);
        Debug.Log("Game Resumed");
    }
}