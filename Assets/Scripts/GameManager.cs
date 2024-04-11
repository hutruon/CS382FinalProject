using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int health;
    public int money;
    private bool gameActive;

    [Header("Components")]
    public TextMeshProUGUI healthAndMoneyText;
    public EnemyPath enemyPath;
    public TowerPlacement towerPlacement;
    public EndScreenUI endScreen;
    public WaveSpawner waveSpawner;

    [Header("Events")]
    public UnityEvent onMoneyChanged;

    // Singleton
    public static GameManager instance;

    // Added variables to control win/lose conditions
    public int wavesToWin = 5; // Number of waves required to win
    public int wavesToGameOver = 10; // Number of waves before game over

    void OnEnable ()
    {
        waveSpawner.OnEnemyRemoved.AddListener(OnEnemyDestroyed);
    }

    void OnDisable ()
    {
        waveSpawner.OnEnemyRemoved.RemoveListener(OnEnemyDestroyed);
    }

    void Awake ()
    {
        instance = this;
    }

    void Start ()
    {
        gameActive = true;
        UpdateHealthAndMoneyText();
    }

    void UpdateHealthAndMoneyText ()
    {
        healthAndMoneyText.text = $"Health: {health}\nMoney: ${money}";
    }

    public void AddMoney (int amount)
    {
        money += amount;
        UpdateHealthAndMoneyText();

        onMoneyChanged.Invoke();
    }

    public void TakeMoney (int amount)
    {
        money -= amount;
        UpdateHealthAndMoneyText();

        onMoneyChanged.Invoke();
    }

    public void TakeDamage (int amount)
    {
        health -= amount;
        UpdateHealthAndMoneyText();

        if(health <= 0)
            GameOver();
    }

    void GameOver ()
    {
        // gameActive = false;
        // endScreen.gameObject.SetActive(true);
        // endScreen.SetEndScreen(false, waveSpawner.curWave);
        SceneManager.LoadScene("EndScreen");
    }

    void WinGame ()
    {
        gameActive = false;
        endScreen.gameObject.SetActive(true);
        endScreen.SetEndScreen(true, waveSpawner.curWave);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnEnemyDestroyed ()
    {
        if(!gameActive)
            return;

        if(waveSpawner.remainingEnemies == 0 && waveSpawner.curWave == waveSpawner.waves.Length)
        {
            if (waveSpawner.curWave >= wavesToWin)
                // WinGame();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else if (waveSpawner.curWave >= wavesToGameOver)
                GameOver();
                
        }
    }
}
