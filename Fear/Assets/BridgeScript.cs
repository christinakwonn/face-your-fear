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

    float gammaValueToLerpTo = 0;
    bool lerpComplete = true;

    float timePassed = 0;
    [SerializeField] float totalTimeToCross = 90f;
    [SerializeField] float bridgeSinkMultiplier = 2f;

    float bridgeSinkNum = 0f;
    [SerializeField] StressMeter stressMeterScript; 
    
    float defaultPlayerYLevel;
    float currentGammaValue = 0;
    [SerializeField] float gammaValueAtThisFrame;

    [Header("Bridge will start swaying a lot more when gamma goes over this value:")]
    [SerializeField] float bridgeSwayThreshold = 3f;
    [Header("Stress meter will start to fill up when gamma goes over this value:")]
    [SerializeField] float stressMeterIncreaseThreshold = 6f;

    [Header("Stress meter increase speed:")]
    [SerializeField] float stressMeterIncreaseMultiplier = 1;
    [Header("Stress meter decrease speed:")]
    [SerializeField] float stressMeterDecreaseMultiplier = 1;
    [SerializeField] GameObject redScreenObj;

    [SerializeField] ParticleSystem[] fogParticles;

    [SerializeField] Color defaultFogColor;
    [SerializeField] Color redFogColor;

    private void Start() {
        centralBridgePosition = transform.position;
        currentGammaValue = gammaValueLerp;
        gammaValueToLerpTo = Receive.gammaValue;
        defaultPlayerYLevel = playerObj.transform.position.y;

        if (stressMeterScript == null) stressMeterScript = FindObjectOfType<StressMeter>();

        stressMeterScript.stressLevel = 0;

        if (redScreenObj == null) redScreenObj = GameObject.Find("RedScreen").transform.gameObject;

        if (redScreenObj != null)
            redScreenObj.SetActive(false);

        if (fogParticles[0] != null) {
            var mainModule = fogParticles[0].main;
            defaultFogColor = mainModule.startColor.color;
        } 
    }

    private void Update() {
        gammaValueAtThisFrame = Receive.gammaValue;

        if (timePassed < totalTimeToCross + 3) // stop moving once crossed 
        {
            sceneObj.transform.position -= new Vector3(0, 0, forwardMovementMultiplier * Time.deltaTime);
            centralBridgePosition -= new Vector3(0, 0, forwardMovementMultiplier * Time.deltaTime);
        }

        // stop swaying the bridge when player is at the end
        if (timePassed > totalTimeToCross - 5) gammaValueToLerpTo = 0;

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

                if (timePassed > totalTimeToCross - (totalTimeToCross / 3)){
                    gammaValueToLerpTo /= 5; // reduce sway as player is nearing end
                    Debug.Log("last third");
                }

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

        if (Receive.gammaValue > stressMeterIncreaseThreshold) // stress meter functionality
        {
            stressMeterScript.stressLevel += stressMeterIncreaseMultiplier * Time.deltaTime;

            //stress meter full, restart
            if (stressMeterScript.stressLevel >= 1) {
                if (redScreenObj != null)
                    redScreenObj.SetActive(true);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else if (stressMeterScript.stressLevel > 0)
            stressMeterScript.stressLevel -= stressMeterDecreaseMultiplier * Time.deltaTime;

        if (fogParticles[0] != null) // lerps fog color from white to red based on stress meter %
        {
            var mainModule = fogParticles[0].main;
            mainModule.startColor = Color.Lerp(defaultFogColor, redFogColor, stressMeterScript.stressLevel);

            var mainModule2 = fogParticles[1].main;
            mainModule2.startColor = Color.Lerp(defaultFogColor, redFogColor, stressMeterScript.stressLevel);
        }
        
        Debug.Log("Gamma: " + Receive.gammaValue);
    }
}
