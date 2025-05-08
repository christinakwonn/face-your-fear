using UnityEngine;
using UnityEngine.SceneManagement;

public class BridgeScript : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject sceneObj;
    [SerializeField] float forwardMovementMultiplier = 10;
    [SerializeField] float bridgeSwayMultiplier;
    Vector3 centralBridgePosition;

    float gammaValueLerp = 0;
    float gammaValueTimer = 0;
    [SerializeField] float smoothFactor = 0.25f;

    float gammaValueToLerpTo = 0;
    bool lerpComplete = true;

    public float timePassed = 0;
    [SerializeField] public float totalTimeToCross = 90f;
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

    [SerializeField] float stressMeterIncreaseMultiplier = 1;
    [SerializeField] float stressMeterDecreaseMultiplier = 1;
    [SerializeField] GameObject redScreenObj;

    [SerializeField] ParticleSystem[] fogParticles;

    [SerializeField] Color defaultFogColor;
    [SerializeField] Color redFogColor;

    private void Start()
    {
        centralBridgePosition = transform.position;
        currentGammaValue = Receive.gammaValue;
        gammaValueToLerpTo = currentGammaValue;

        defaultPlayerYLevel = playerObj.transform.position.y;

        if (stressMeterScript == null)
            stressMeterScript = Object.FindFirstObjectByType<StressMeter>();

        stressMeterScript.stressLevel = 0;

        if (redScreenObj != null)
            redScreenObj.SetActive(false);

        if (fogParticles.Length > 0 && fogParticles[0] != null)
        {
            var main = fogParticles[0].main;
            defaultFogColor = main.startColor.color;
        }
    }

    private void Update()
    {
        gammaValueAtThisFrame = Receive.gammaValue;

        // Move the scene and bridge over time
        if (timePassed < totalTimeToCross + 3f)
        {
            Vector3 moveDelta = new Vector3(0, 0, forwardMovementMultiplier * Time.deltaTime);
            sceneObj.transform.position -= moveDelta;
            centralBridgePosition -= moveDelta;
        }

        // Stop gammaValue influence near the end
        if (timePassed > totalTimeToCross - 5f)
            gammaValueToLerpTo = 0;

        // Smooth gamma lerping for bridge sway
        if (Time.time > 2f)
        {
            if (!Mathf.Approximately(gammaValueLerp, gammaValueToLerpTo))
            {
                gammaValueTimer += Time.deltaTime * smoothFactor;
                gammaValueLerp = Mathf.Lerp(currentGammaValue, gammaValueToLerpTo, gammaValueTimer);

                if (gammaValueTimer >= 1f)
                {
                    lerpComplete = true;
                    gammaValueTimer = 0;
                    currentGammaValue = gammaValueLerp;
                }
            }

            if (lerpComplete)
            {
                lerpComplete = false;
                gammaValueToLerpTo = Receive.gammaValue;

                // Reduce sway when nearing the end of the bridge
                if (timePassed > totalTimeToCross * 2f / 3f)
                    gammaValueToLerpTo /= 5f;

                if (gammaValueToLerpTo < bridgeSwayThreshold)
                    gammaValueToLerpTo = 0.5f;
            }
        }

        float swayAmount = Mathf.Sin(Time.time) * gammaValueLerp * bridgeSwayMultiplier;
        transform.position = centralBridgePosition + new Vector3(swayAmount, 0, 0);

        // Sink bridge in middle and return it to normal
        if (timePassed < totalTimeToCross / 2f)
            bridgeSinkNum = Mathf.Lerp(0, 1, timePassed / (totalTimeToCross / 2f));
        else
            bridgeSinkNum = Mathf.Lerp(1, 0, (timePassed - totalTimeToCross / 2f) / (totalTimeToCross / 2f));

        playerObj.transform.position = new Vector3(
            centralBridgePosition.x,
            defaultPlayerYLevel - bridgeSinkNum * bridgeSinkMultiplier,
            playerObj.transform.position.z) + new Vector3(swayAmount, 0, 0);

        timePassed += Time.deltaTime;

        // Stress meter logic
        if (Receive.gammaValue > stressMeterIncreaseThreshold)
        {
            stressMeterScript.stressLevel += stressMeterIncreaseMultiplier * Time.deltaTime;

            if (stressMeterScript.stressLevel >= 1f)
            {
                if (redScreenObj != null)
                    redScreenObj.SetActive(true);

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        else
        {
            stressMeterScript.stressLevel = Mathf.Max(0, stressMeterScript.stressLevel - stressMeterDecreaseMultiplier * Time.deltaTime);
        }

        // Fog color based on stress
        if (fogParticles.Length >= 2 && fogParticles[0] != null && fogParticles[1] != null)
        {
            var main1 = fogParticles[0].main;
            var main2 = fogParticles[1].main;

            Color lerpedColor = Color.Lerp(defaultFogColor, redFogColor, stressMeterScript.stressLevel);
            main1.startColor = lerpedColor;
            main2.startColor = lerpedColor;
        }

        Debug.Log("Gamma: " + Receive.gammaValue);
    }
}
