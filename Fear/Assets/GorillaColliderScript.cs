using UnityEngine;
using UnityEngine.SceneManagement;

public class GorillaColliderScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Gorilla"){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
