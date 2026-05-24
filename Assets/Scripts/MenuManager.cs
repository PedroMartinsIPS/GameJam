using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static string currentLevel = "Nivel1";

    public void StartGame()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MenuGame(){
        SceneManager.LoadScene("Menu");
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(currentLevel);
    }
}