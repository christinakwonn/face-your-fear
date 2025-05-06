using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{
    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Level1"); // Replace with your scene name
    }
}
