using UnityEngine;

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

    [SerializeField] float bridgeSinkNum = 0f;
    [SerializeField] StressMeter stressMeterScript; 
    
    float defaultPlayerYLevel;

    [SerializeField] float stressMeterFillMultiplier = 1;


    private void Start() {
        centralBridgePosition = transform.position;
        currentGammaValue = gammaValueLerp;
        gammaValueToLerpTo = Receive.gammaValue;
        defaultPlayerYLevel = playerObj.transform.position.y;

        if (stressMeterScript == null) stressMeterScript = FindObjectOfType<StressMeter>();
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
                if (gammaValueToLerpTo < 3.7f) gammaValueToLerpTo = 0.5f; // 3.7 seems like a "baseline" value
                //Debug.Log("Current sway value at: " + gammaValueToLerpTo);
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
        Debug.Log(stressMeterScript.stressLevel);
    }
}
