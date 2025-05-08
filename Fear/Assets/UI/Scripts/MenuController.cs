using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame(string Level1)
    {
        SceneManager.LoadScene(Level1);
    }

    public void OpenTutorial(string TutorialScene)
    {
        SceneManager.LoadScene(TutorialScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}