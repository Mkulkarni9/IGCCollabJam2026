using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool IsGameStarted { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsGamePaused { get; private set; }

    public static Action OnClickUIButton;
    public static Action OnGameSceneLoaded;
    public static Action OnStartGame;
    public static Action OnGamePaused;
    public static Action OnGameUnpaused;

    [SerializeField] Image winScreen;
    [SerializeField] Image loseScreen;
    [SerializeField] Image instructionsScreen;
    [SerializeField] TextMeshProUGUI winMessageText;
    [SerializeField] TextMeshProUGUI loseMessageText;
    [SerializeField] Button pauseGameButton;
    [SerializeField] Button resumeGameButton;


    [SerializeField] TextMeshProUGUI winStatsText;
    [SerializeField] TextMeshProUGUI loseStatsText;



    int currentSceneIndex;



    void Awake()
    {
        Time.timeScale = 1f;
    }



    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        
    }
    

    void OnEnable()
    {
    
    }

    void OnDisable()
    {
        
    }

    #region Game state methods
    public void StartGame()
    {
        IsGameStarted = true;
        IsGameOver = false;
        Time.timeScale = 1f;
        OnStartGame?.Invoke();
    }
    
    public void WinGame()
    {

        IsGameOver = true;
        winScreen.gameObject.SetActive(true);
        
        Time.timeScale = 0f;

    }
    
    public void LoseGame()
    {

        IsGameOver = true;
        loseScreen.gameObject.SetActive(true);
        
        Time.timeScale = 0f;

    }

    void TogglePause()
    {


        if(!IsGameStarted) { return; }
        
        if(IsGamePaused)
        {
            Time.timeScale = 1f;
            IsGamePaused = false;
            OnGameUnpaused?.Invoke();
        }
        else
        {
            Time.timeScale = 0f;
            IsGamePaused = true;
            OnGamePaused?.Invoke();
        }

    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        IsGamePaused = true;
        OnGamePaused?.Invoke();
    }

    

    #endregion




    #region ButtonMethods

    public void PlayGameButton()
    {
        OnClickUIButton?.Invoke();
        StartGame();
    }

    public void RestartCurrentSceneButton()
    {
        OnClickUIButton?.Invoke();
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void GoToSceneButton(int sceneIndex)
    {
        OnClickUIButton?.Invoke();
        SceneManager.LoadScene(sceneIndex);
    }

    public void TogglePauseButton()
    {
        OnClickUIButton?.Invoke();
        TogglePause();
    }
    




    public void Exit()
    {
        OnClickUIButton?.Invoke();
        Debug.Log("Quit Game");
        Application.Quit();
    }

    
    #endregion


}
