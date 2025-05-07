using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Level1");
    }
}
