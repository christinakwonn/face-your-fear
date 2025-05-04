using UnityEngine;

public class BridgeScript : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject sceneObj;
    [SerializeField] float forwardMovementMultiplier = 10;
    [SerializeField] float bridgeSwayMultiplier;
    Vector3 startingBridgePosition;

    float gammaValueLerp = 0;
    float gammaValueTimer = 0;
    [SerializeField] float smoothFactor = 0.25f;
    float currentGammaValue = 0;
    float gammaValueToLerpTo = 0;
    bool lerpComplete = true;

    private void Start() {
        //playerObj.transform.position = sceneObj.transform.position;
        startingBridgePosition = transform.position;
        currentGammaValue = gammaValueLerp;
        gammaValueToLerpTo = Receive.gammaValue;
    }

    private void Update() {
        sceneObj.transform.position -= new Vector3(0, 0, forwardMovementMultiplier * Time.deltaTime);

        if (Time.time > 2){
            if (gammaValueLerp != Receive.gammaValue){
                gammaValueTimer += Time.deltaTime * smoothFactor;
                gammaValueLerp = Mathf.Lerp(currentGammaValue, Receive.gammaValue, gammaValueTimer);
                if (gammaValueTimer >= 1 * smoothFactor) lerpComplete = true;
            }
            else if (lerpComplete){
                gammaValueTimer = 0;
                currentGammaValue = gammaValueLerp;
                gammaValueToLerpTo = Receive.gammaValue;
            }
        }

        float swayAmount = Mathf.Sin(Time.time) * gammaValueLerp * bridgeSwayMultiplier;
        Debug.Log(swayAmount);
        transform.transform.position += /*startingBridgePosition*/ new Vector3(swayAmount, 0, 0);
        playerObj.transform.position += /*startingBridgePosition*/ new Vector3(swayAmount, 0, 0);
    }
}
