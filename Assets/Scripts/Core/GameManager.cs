using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState currentState;

    void Awake()
    {
       if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateState(GameState.Playing);
    }

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing) 

            UpdateState(GameState.Pause);
            
            else if (currentState == GameState.Pause) 

            UpdateState(GameState.Playing);
        }
    }

   public void UpdateState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Pause:
                Time.timeScale = 0f; // Menghentikan game
                break;
            case GameState.GameOver:
                Time.timeScale = 0f; // Menghentikan game saat kalah
                break;
        }
        Debug.Log("State berubah ke: " + newState);
    }

    public void GameOver() 
{
    UpdateState(GameState.GameOver);
}
}