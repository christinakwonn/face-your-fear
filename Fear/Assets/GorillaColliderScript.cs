using UnityEngine;
using UnityEngine.SceneManagement;

public class GorillaColliderScript : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip squishSound;
    bool canRestart = false;
    float restartTimer = 0;

    private void OnEnable() {
        restartTimer = 0;
        canRestart = false;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Gorilla"){
            if (!canRestart){
                if (audioSource != null)
                    audioSource.PlayOneShot(squishSound);
            }
            canRestart = true;
            
        }
    }
    private void Update() {
        if (canRestart){
            restartTimer += Time.deltaTime;
            if (restartTimer > 1.3f){
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            
        }
    }
}
