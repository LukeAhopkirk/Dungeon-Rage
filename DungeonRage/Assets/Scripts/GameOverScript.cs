using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }

    public void RestartEndlessMode()
    {
        SceneManager.LoadScene("EndlessMode"); 
    }
}
