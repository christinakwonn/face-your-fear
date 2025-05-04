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


    private void Start() {
        //playerObj.transform.position = sceneObj.transform.position;
        centralBridgePosition = transform.position;
        currentGammaValue = gammaValueLerp;
        gammaValueToLerpTo = Receive.gammaValue;
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
                Debug.Log("Current sway value at: " + gammaValueToLerpTo);
            }
        }

        float swayAmount = Mathf.Sin(Time.time) * gammaValueLerp * bridgeSwayMultiplier;
        transform.position = centralBridgePosition + new Vector3(swayAmount, 0, 0);
        playerObj.transform.position = new Vector3(centralBridgePosition.x, playerObj.transform.position.y, playerObj.transform.position.z) + new Vector3(swayAmount, 0, 0);
    }
}
