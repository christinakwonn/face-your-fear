using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public void NextScene(string Level1)
    {
        SceneManager.LoadScene(Level1);
    }
}