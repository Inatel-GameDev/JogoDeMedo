using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Action,
        Loading
    }


    public enum Scene
    {
        StartMenu,
        Alves,
        Dju,
        Marcelo,
        Geral
    }

    public static GameManager Instance;
    public GameState gameState;
    public Canvas hud;
    [SerializeField] private Jogador _jogador;


    private void Awake()
    {
        Instance = this;
    }


    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    // public void PauseGame()
    // {
    //     Cursor.lockState = CursorLockMode.Confined;
    //     Cursor.visible = true;
    //     gameState = GameState.Pause;
    //     //player.IsPaused = true;
    //     Time.timeScale = 0f;
    //     menu_pause.gameObject.SetActive(true);
    // }

//     public void ResumeGame()
//     {
//         Cursor.lockState = CursorLockMode.Locked;
//         Cursor.visible = false;
//         gameState = GameState.Action;
// //        player.IsPaused = false;
//         Time.timeScale = 1f;
//         menu_pause.gameObject.SetActive(false);
//     }
    
}