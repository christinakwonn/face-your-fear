using UnityEngine;
using UnityEngine.SceneManagement;
public class GorillaColliderScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider hit) {
        if (hit.tag == "Gorilla"){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
