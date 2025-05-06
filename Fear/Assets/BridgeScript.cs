using UnityEngine;
using UnityEngine.SceneManagement;
public class BridgeScript : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject sceneObj;
    [SerializeField] float forwardMovementMultiplier = 10;
    [SerializeField] float bridgeSwayMultiplier;
    Vector3 centralBridgePosition;

    //float gammaValueTemp = 0;
    float gammaValueLerp = 0;
    float gammaValueTimer = 0;
    [SerializeField] float smoothFactor = 0.25f;
    float currentGammaValue = 0;
    float gammaValueToLerpTo = 0;
    bool lerpComplete = true;

    float timePassed = 0;
    [SerializeField] float totalTimeToCross = 90f;
    [SerializeField] float bridgeSinkMultiplier = 2f;

    float bridgeSinkNum = 0f;
    [SerializeField] StressMeter stressMeterScript; 
    
    float defaultPlayerYLevel;

    [Header("Bridge will start swaying a lot more when gamma goes over this value:")]
    [SerializeField] float bridgeSwayThreshold = 3f;
    [Header("Stress meter will start to fill up when gamma goes over this value:")]
    [SerializeField] float stressMeterIncreaseThreshold = 6f;

    [Header("Stress meter increase speed:")]
    [SerializeField] float stressMeterIncreaseMultiplier = 1;
    [Header("Stress meter decrease speed:")]
    [SerializeField] float stressMeterDecreaseMultiplier = 1;


    private void Start() {
        centralBridgePosition = transform.position;
        currentGammaValue = gammaValueLerp;
        gammaValueToLerpTo = Receive.gammaValue;
        defaultPlayerYLevel = playerObj.transform.position.y;

        if (stressMeterScript == null) stressMeterScript = FindObjectOfType<StressMeter>();

        stressMeterScript.stressLevel = 0;
    }

    private void Update() {
        sceneObj.transform.position -= new Vector3(0, 0, forwardMovementMultiplier * Time.deltaTime);
        centralBridgePosition -= new Vector3(0, 0, forwardMovementMultiplier * Time.deltaTime);

        if (Time.time > 2){
            if (gammaValueLerp != gammaValueToLerpTo){
                gammaValueTimer += Time.deltaTime * smoothFactor;
                gammaValueLerp = Mathf.Lerp(currentGammaValue, gammaValueToLerpTo, gammaValueTimer);
                if (gammaValueTimer >= 1 * smoothFactor) lerpComplete = true;
            }
            else if (lerpComplete){
                gammaValueTimer = 0;
                currentGammaValue = gammaValueLerp;
                gammaValueToLerpTo = Receive.gammaValue;
                if (gammaValueToLerpTo < bridgeSwayThreshold) gammaValueToLerpTo = 0.5f; 
            }
        }

        float swayAmount = Mathf.Sin(Time.time) * gammaValueLerp * bridgeSwayMultiplier;
        transform.position = centralBridgePosition + new Vector3(swayAmount, 0, 0);
        playerObj.transform.position = new Vector3(centralBridgePosition.x, defaultPlayerYLevel - bridgeSinkNum * bridgeSinkMultiplier, playerObj.transform.position.z) + new Vector3(swayAmount, 0, 0);

        if (timePassed < totalTimeToCross / 2f){
            bridgeSinkNum = Mathf.Lerp(0, 1, timePassed / (totalTimeToCross / 2f));
        }
        else if (timePassed > totalTimeToCross / 2f){
            bridgeSinkNum = Mathf.Lerp(1, 0, (timePassed - totalTimeToCross / 2f) / (totalTimeToCross / 2f));
        }

        timePassed += Time.deltaTime;

        if (Receive.gammaValue > stressMeterIncreaseThreshold) {
            stressMeterScript.stressLevel += stressMeterIncreaseMultiplier * Time.deltaTime;
            if (stressMeterScript.stressLevel >= 1) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (stressMeterScript.stressLevel > 0)
            stressMeterScript.stressLevel -= stressMeterDecreaseMultiplier * Time.deltaTime;
        
        Debug.Log(Receive.gammaValue);
    }
}
