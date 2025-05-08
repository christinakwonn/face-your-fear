using UnityEngine;

public class EndScreenButtonEnableDelay : MonoBehaviour
{
    [SerializeField] GameObject canvasObj;
    [SerializeField] float buttonDelay;
    float timer = 0;

    void Start (){
        canvasObj.SetActive(false);
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > 3){
            canvasObj.SetActive(true);
        }
    }

}
