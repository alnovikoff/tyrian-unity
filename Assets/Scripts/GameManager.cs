using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Material skybox;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject pauseUI;

    [SerializeField] TMP_Text gameGoals;
    [SerializeField] TMP_Text currentGameGoals;
    public int gamesGoalsSet;
    public int gameGloasCounter;

    bool isGameover = false;
    bool isGamePaused = false;

    [SerializeField] private GameObject joyStickObject;
    [SerializeField] private GameObject cheatUI;
    [SerializeField] private GameObject pauseButton;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
#if UNITY_ANDROID
        joyStickObject.SetActive(true);
        pauseButton.SetActive(true);
        cheatUI.SetActive(false);
#else
        joyStickObject.SetActive(false);
        pauseButton.SetActive(false);
        cheatUI.SetActive(true);
#endif
        Time.timeScale = 1.0f;
        pauseUI.SetActive(false);   
        isGameover = false;
        isGamePaused = false;
        if (SceneManager.GetActiveScene().name == "level1")
        {
            gameGoals.text = "Destroy 10 meteors";
            gamesGoalsSet = 10;
        }
        else if (SceneManager.GetActiveScene().name == "level2")
        {
            gameGoals.text = "Destroy 15 vessels";
            gamesGoalsSet = 12;
        }
        else if (SceneManager.GetActiveScene().name == "level3")
        {
            gameGoals.text = "Fight boss";
            gamesGoalsSet = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SwitchLevels();
        if (UIManager.Instance.playerBase.currentHealth <= 0)
        {
            Time.timeScale = 0;
            isGameover = true;
            gameOverUI.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "level1" &&  gameGloasCounter >= gamesGoalsSet)
        {
            SceneManager.LoadScene("level2");
        }
        else if (SceneManager.GetActiveScene().name == "level2" && gameGloasCounter >= gamesGoalsSet)
        {
            SceneManager.LoadScene("level3");
        }
        else if (SceneManager.GetActiveScene().name == "level3" && gameGloasCounter >= gamesGoalsSet)
        {
            Time.timeScale = 0;
            UIManager.Instance.WinUI();
        }
#if UNITY_STANDALONE
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (isGameover)
            {
                gameOverUI.SetActive(false);
                isGameover = false;
                Time.timeScale = 1;
                SceneManager.LoadScene("level1");
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!isGamePaused)
            {
                isGamePaused = true;
                pauseUI.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                isGamePaused = false;
                pauseUI.SetActive(false);
                Time.timeScale = 1;
            }
        }
#endif
        if (SceneManager.GetActiveScene().name == "level1")
        {
            currentGameGoals.text = gameGloasCounter.ToString() + " / " + gamesGoalsSet.ToString() + " meteors were destroyed";
        }
        else if (SceneManager.GetActiveScene().name == "level2")
        {
            currentGameGoals.text = gameGloasCounter.ToString() + " / " + gamesGoalsSet.ToString() + " enemies were destroyed";
        }
        else if (SceneManager.GetActiveScene().name == "level3")
        {
            currentGameGoals.text = gameGloasCounter.ToString() + " / " + gamesGoalsSet.ToString() + " enemy was destroyed";
        }
    }

    public void PauseGame()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            isGamePaused = false;
            pauseUI.SetActive(false);
            Time.timeScale = 1;
        }

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("main_menu");
    }

    private void SwitchLevels()
    {
        if(Input.GetKeyUp(KeyCode.N))
        {
            if (SceneManager.GetActiveScene().name == "level1")
            {
                SceneManager.LoadScene("level2");
            }
            else if (SceneManager.GetActiveScene().name == "level2")
            {
                SceneManager.LoadScene("level3");
            }
            else
            {
                LoadMainMenu();
            }
        }
    }
}
