using UnityEngine;
using UnityEngine.UI;

public class Challenge2Handling : MonoBehaviour
{
    [SerializeField] Toggle challengeToggle;
    [SerializeField] BridgeScript bridgeScript;
    [SerializeField] bool challengeTwoComplete = false;

    private void Update() {
        if (bridgeScript.timePassed > bridgeScript.totalTimeToCross) // player reached the end of bridge, complete challenge
            challengeTwoComplete = true;

        if (challengeTwoComplete) challengeToggle.isOn = true;
    }
}
